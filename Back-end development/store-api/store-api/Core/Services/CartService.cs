using Microsoft.Extensions.Logging;
using store_api.Core.DTOs.Requests;
using store_api.Core.DTOs.Responses;
using store_api.Core.Exceptions;
using store_api.Core.Helpers;
using store_api.Core.Interfaces;
using store_api.Core.Interfaces.Services;
using store_api.Core.Models;
using System;
using System.Linq;

namespace store_api.Core.Services
{
    public class CartService : BaseService, ICart
    {
        private readonly ILogger<CartService> _logger;


        public CartService(ILogger<CartService> logger, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _logger = logger;
        }

        public ResponseWithData<AddToCartData> AddToCart(AddToCartRequest request)
        {
            try
            {
                if (request.Quantity < 0) throw new CustomException("Invalid quantity provided");


                var cartsession = new Cart();
                bool sessionexist = false;

                if (string.IsNullOrEmpty(request.SessionId))
                {
                    var sessionid = CommonHelper.GenerateSession();
                    cartsession = _work.CartRepository.NewCart(sessionid, DateTime.UtcNow.AddMinutes(60), request.Ip);
                }
                else
                {
                    cartsession = _work.CartRepository.FindWithSessionId(request.SessionId);
                    if (cartsession == null)
                    {
                        throw new CustomException("No session exists with the session id shared");
                    }

                    sessionexist = true;
                }

                var product = _work.ProductRepository.Find(request.ProductId);
                if (product == null) throw new CustomException("product does not exist");

                //add cart item history abeg for data science sake

                //check if item already in cart and just increase the item
                var cartitem = _work.CartItemRepository.FindByQuery("select * from cartitems where cartid={0} and productid={1}", new object[] { cartsession.Id, product.Id }).FirstOrDefault();

                if (cartitem != null)
                {
                    if (request.Type == "increase")
                    {
                        cartitem.Quantity = cartitem.Quantity + request.Quantity;
                        cartitem.Price = product.Price;
                        _work.CartItemRepository.Update(cartitem);
                        _work.Commit();
                    }
                    else
                    {
                        if (cartitem.Quantity > request.Quantity) throw new CustomException("quantity to set is less than or equal to current quantity");
                        cartitem.Quantity = request.Quantity;
                        cartitem.Price = product.Price;
                        _work.CartItemRepository.Update(cartitem);

                        _work.Commit();

                    }

                    var ccartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                    var ddata = new ViewCartDetails { Items = ccartitems };

                    decimal ttotalamount = 0;

                    foreach (var i in ccartitems)
                    {
                        ttotalamount += (i.Price * i.Quantity);
                    }

                    ddata.TotalAmount = ttotalamount.ToString();

                    return new ResponseWithData<AddToCartData> { Message = "item added to cart successfully", Data = new AddToCartData { SessionId = cartsession.SessionId, CartData = ddata } };
                }

                decimal price = product.Price;

                var item = _work.CartItemRepository.AddCartItem(cartsession.Id, price, product.Id, request.Quantity, null, 0);

                var cartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                var data = new ViewCartDetails { Items = cartitems };

                data.TotalAmount = cartitems.Sum(x => (x.Price * x.Quantity)).ToString();

                return new ResponseWithData<AddToCartData> { Message = "item added to cart successfully", Data = new AddToCartData { SessionId = cartsession.SessionId, CartData = data } };
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("Unable to add item to cart at this time");
            }

        }

        public ResponseWithData<RemoveFromCartData> RemoveFromCart(RemoveFromCartRequest request)
        {
            try
            {
                if (request.Quantity < 0) throw new CustomException("invalid quantity cannot be added to cart.");

                var cartsession = _work.CartRepository.FindWithSessionId(request.SessionId);
                if (cartsession == null)
                {
                    throw new CustomException("your previous session is expired.");
                }

                var product = _work.ProductRepository.Find(request.ProductId);
                if (product == null)
                {
                    throw new CustomException("Store does not own product shared");
                }
                //add cart item history abeg for data science sake

                var cartitem = _work.CartItemRepository.FindByQuery("select * from cartitems where cartid={0} and productid={1}", new object[] { cartsession.Id, product.Id }).FirstOrDefault();

                if (cartitem != null)
                {
                    if (request.Type == "decrease")
                    {
                        cartitem.Quantity = cartitem.Quantity - request.Quantity;
                        if (cartitem.Quantity < 0) throw new CustomException("item not currently in your cart");

                        _work.CartItemRepository.Update(cartitem);

                        _work.Commit();
                    }
                    else
                    {
                        if (cartitem.Quantity < request.Quantity) throw new CustomException("cannot remove quantity of item from cart");

                        cartitem.Quantity = request.Quantity;
                        _work.CartItemRepository.Update(cartitem);

                        _work.Commit();

                    }

                    var cartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                    var data = new ViewCartDetails { Items = cartitems };

                    data.TotalAmount = cartitems.Sum(x => (x.Price * x.Quantity)).ToString();

                    return new ResponseWithData<RemoveFromCartData> { Message = "item removed from cart successfully", Data = new RemoveFromCartData { SessionId = cartsession.SessionId, CartData = data } };
                }

                var ccartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                var ddata = new ViewCartDetails { Items = ccartitems };

                decimal ttotalamount = 0;

                foreach (var i in ccartitems)
                {
                    ttotalamount += (i.Price * i.Quantity);
                }

                ddata.TotalAmount = ttotalamount.ToString();

                return new ResponseWithData<RemoveFromCartData> { Message = "item not present in cart", Status = "failed", StatusCode = "02", Data = new RemoveFromCartData { SessionId = cartsession.SessionId, CartData = ddata } };

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("Unable to remove from cart at this time");
            }
        }

        public ResponseWithData<ViewCartDetails> ViewCart(string sessionid)
        {
            try
            {
                var cartsession = _work.CartRepository.FindWithSessionId(sessionid);
                if (cartsession == null)
                {
                    throw new CustomException("No session exists with the session id shared");
                }

                var cartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                var data = new ViewCartDetails { Items = cartitems };

                data.TotalAmount = cartitems.Sum(x => (x.Price * x.Quantity)).ToString();
                return new ResponseWithData<ViewCartDetails> { Message = "cart items fetched successfully", Data = data };

            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("Unable to view cart at this time");
            }
        }

        public Response SaveCartForLater(SaveForLaterRequest request)
        {
            try
            {
                var cartsession = _work.CartRepository.FindWithSessionId(request.SessionId);
                if (cartsession == null)
                {
                    throw new CustomException("No session exists with the session id shared");
                }

                var customer = _work.CustomerRepository.GetCustomer(request.Identifier);
                if (customer == null) throw new CustomException("Sorry, your account not found at this time");

                cartsession.SavedForLater = true;
                cartsession.DateUpdated = DateTime.UtcNow;
                cartsession.CustomerId = customer.Id;

                _work.CartRepository.Update(cartsession);
                _work.Commit();

                return new Response { Message = "cart successfully saved for later" };
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("sorry, currently unable to save cart for later");
            }

        }

        public ResponseWithData<AddToCartData> AddTBulkToCart(AddBulkCartRequest request)
        {
            try
            {
                var cartsession = new Cart();
                bool sessionexist = false;

                if (string.IsNullOrEmpty(request.SessionId))
                {
                    var sessionid = CommonHelper.GenerateSession();
                    cartsession = _work.CartRepository.NewCart(sessionid, DateTime.UtcNow.AddMinutes(60), request.Ip);
                }
                else
                {
                    cartsession = _work.CartRepository.FindWithSessionId(request.SessionId);
                    if (cartsession == null)
                    {
                        throw new CustomException("No session exists with the session id shared");
                    }
                    sessionexist = true;
                }

                foreach (var req in request.Products)
                {
                    if (req.Quantity < 0) throw new CustomException("Invalid quantity provided");

                    var product = _work.ProductRepository.Find(req.ProductId);
                    if (product == null)
                    {
                        throw new CustomException("Store does not own product shared");
                    }
                    //add cart item history abeg for data science sake

                    //check if item already in cart and just increase the item
                    var cartitem = _work.CartItemRepository.FindByQuery("select * from cartitems where cartid={0} and productid={1}", new object[] { cartsession.Id, product.Id }).FirstOrDefault();

                    if (cartitem != null)
                    {
                        if (req.Type == "increase")
                        {
                            cartitem.Quantity = cartitem.Quantity + req.Quantity;
                            cartitem.Price = product.Price;
                            _work.CartItemRepository.Update(cartitem);

                            _work.Commit();
                        }
                        else
                        {
                            if (cartitem.Quantity > req.Quantity) throw new CustomException("quantity to set is less than or equal to current quantity");
                            cartitem.Quantity = req.Quantity;
                            cartitem.Price = product.Price;
                            _work.CartItemRepository.Update(cartitem);

                            _work.Commit();

                        }

                        var ccartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                        var ddata = new ViewCartDetails { Items = ccartitems };

                        ddata.TotalAmount = ccartitems.Sum(x => (x.Price * x.Quantity)).ToString();

                        return new ResponseWithData<AddToCartData> { Message = "item added to cart successfully", Data = new AddToCartData { SessionId = cartsession.SessionId, CartData = ddata } };
                    }

                    var item = _work.CartItemRepository.AddCartItem(cartsession.Id, product.Price, product.Id, req.Quantity, null, 0);

                }

                var cartitems = _work.CartItemRepository.GetCartItems(cartsession.Id).Where(x => x.Quantity > 0).ToList();
                var data = new ViewCartDetails { Items = cartitems };

                data.TotalAmount = cartitems.Sum(x => (x.Price * x.Quantity)).ToString();

                return new ResponseWithData<AddToCartData> { Message = "items added to cart successfully", Data = new AddToCartData { SessionId = cartsession.SessionId, CartData = data } };
            }
            catch (Exception ex)
            {
                if (ex is CustomException) throw;
                throw new CustomException("Unable to add item to cart at this time");
            }
        }
    }
}
