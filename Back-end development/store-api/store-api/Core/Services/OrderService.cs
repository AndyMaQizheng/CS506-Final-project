using Microsoft.Extensions.Logging;
using store_api.Core.DTOs.Requests;
using store_api.Core.DTOs.Responses;
using store_api.Core.Enums;
using store_api.Core.Exceptions;
using store_api.Core.Helpers;
using store_api.Core.Interfaces;
using store_api.Core.Interfaces.Services;
using store_api.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace store_api.Core.Services
{
    public class OrderService : BaseService, IOrder
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IToken _token;

        public OrderService(ILogger<OrderService> logger, IToken token, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _logger = logger;
            _token = token;
        }

        public Response CompleteOrder(CompleteOrderRequest request)
        {
            try
            {
                var order = _work.OrderRepository.FindByQuery("select * from orders where orderreference={0}", new object[] { request.OrderReference }).FirstOrDefault();
                if (order == null) throw new CustomException("Order not found");
                if (order.OrderStatusId != (int)OrderStatusEnum.InProgress) throw new CustomException("sorry, order previously completed");

                var customer = GetCustomer(request.CustomerId);

                order.ShippingAddress = request.ShippingAddress;
                order.OrderStatusId = (int)OrderStatusEnum.Successful;

                _work.OrderRepository.Update(order);
                _work.Commit();

                return new Response { Message = "Order completed successfully" };
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("Unable to complete order at this time");
            }
        }

        public ResponseWithData<AuthenticateOrderData> AuthenticateOrder(AuthenticateOrderRequest request)
        {
            try
            {
                var order = _work.OrderRepository.FindByReference(request.OrderReference);
                if (order == null) throw new CustomException("Order not found");
                if (order.OrderStatusId != (int)OrderStatusEnum.Initiated) throw new CustomException("Sorry, your order has been previously completed.");

                CheckIfSessionAlreadyPaidFor(order.SessionId);

                var checkoutInteg = new CheckOutIntegration();
                var orderpayment = checkoutInteg.VerifyPayment(order.OrderReference);

                if (orderpayment == null) throw new CustomException("Sorry, could not verify your payment at this time. please try again");
                if (orderpayment.status == "failed") throw new CustomException(orderpayment.message);

                var orderdata = orderpayment.data;

                if (orderdata.response_code == "00" || orderdata.response_code == "01")
                {
                    var customer = _work.CustomerRepository.GetCustomer(orderdata.customer_email_address);
                    if (customer == null) throw new CustomException("sorry, your profile does not exist on apace");

                    order.OrderStatusId = (int)OrderStatusEnum.InProgress;
                    order.CustomerId = customer.Id;

                    //send email to merchant that order successful blah blah
                    _work.OrderRepository.Update(order);
                    _work.Commit();

                    string token = _token.GenerateTokenString(customer.Id, customer.EmailAddress);
                    _token.SaveToken(customer.Id, token);

                    var data = new AuthenticateOrderData
                    {
                        Token = token,
                    };

                    return new ResponseWithData<AuthenticateOrderData> { Message = "order authenticated successfully", Data = data };
                }

                order.OrderStatusId = (int)OrderStatusEnum.Failed;
                _work.OrderRepository.Update(order);
                _work.Commit();

                throw new CustomException("Sorry, your payment for order was not successful. please try again.");
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("sorry, unable to complete order at this time. please contact support if this persists");
            }
        }

        public void CheckIfSessionAlreadyPaidFor(string sessionid)
        {
            var sessioncheckedoutbefore = _work.OrderRepository.FindByQuery("select * from orders where sessionid={0} and (orderstatusid=5 or orderstatusid=2)", new object[] { sessionid }).FirstOrDefault();
            if (sessioncheckedoutbefore != null) throw new CustomException("Initiate new order to continue, session order previously completed");
        }
        public ResponseWithData<OrderResponseData> InitiateOrder(InitiateOrderRequest request)
        {
            try
            {
                var cartsession = _work.CartRepository.FindWithSessionId(request.SessionId);
                if (cartsession == null)
                {
                    throw new CustomException("sorry, your previous session has expired.");
                }

                CheckIfSessionAlreadyPaidFor(request.SessionId);
                var order = _work.OrderRepository.NewOrder(request.SessionId);

                var orderitems = new List<OrderItem> { };
                decimal totalamount = 0;

                //when initiating order have to check if that thing is still in stock lmao TODODODODODODODODOD
                //TODO: add when user intiates order reference to know number of trials
                if (request.Items == null)
                {
                    var cartitems = _work.CartItemRepository.FindCartItems(cartsession.Id);
                    if (cartitems.Count <= 0) throw new CustomException("Session Id contains no items");
                    foreach (var item in cartitems)
                    {
                        var it = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Discount = 0,
                            Price = item.Price
                        };

                        orderitems.Add(it);
                        var itemordertotal = item.Price * item.Quantity;

                        totalamount += itemordertotal;
                    }
                }
                else
                {
                    if (request.Items.Count <= 0)
                    {
                        //instead of this use sessionid to get the items in the cart previously TODODODODODODODODDODO
                        var cartitems = _work.CartItemRepository.FindCartItems(cartsession.Id);
                        foreach (var item in cartitems)
                        {
                            var it = new OrderItem
                            {
                                OrderId = order.Id,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                Discount = 0,
                                Price = item.Price
                            };

                            orderitems.Add(it);
                            var itemordertotal = item.Price * item.Quantity;

                            totalamount += itemordertotal;
                        }
                    }
                    else
                    {
                        foreach (var item in request.Items)
                        {
                            var product = _work.ProductRepository.Find(item.ProductId);
                            if (product != null)
                            {
                                decimal price = product.Price;

                                var it = new OrderItem
                                {
                                    OrderId = order.Id,
                                    ProductId = item.ProductId,
                                    Quantity = item.Quantity,
                                    Discount = 0,
                                    Price = product.Price,
                                    OnDiscount = false
                                };

                                orderitems.Add(it);
                                var itemordertotal = product.Price * item.Quantity;

                                totalamount += itemordertotal;
                            }
                        }
                    }

                }

                var orderreference = CommonHelper.GenerateRef("order");
                order.OrderReference = orderreference;
                order.TotalAmount = totalamount;
                order.CurrencyId = 1;
                order.OrderStatusId = (int)OrderStatusEnum.Initiated;
                _work.OrderRepository.Update(order);

                _work.Commit();

                var ccartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                var ddata = new ViewCartDetails { Items = ccartitems, TotalAmount = totalamount.ToString() };

                _work.OrderItemRepository.InsertBulkOrderItems(orderitems);
                var data = new OrderResponseData { OrderReference = orderreference, OrderData = ddata };

                return new ResponseWithData<OrderResponseData> { Message = "Order initiated successfully", Data = data };
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("Unable to initaite order at this time");
            }
        }
    }
}
