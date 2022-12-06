using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_api.Core.Attributes;
using store_api.Core.DTOs.Requests;
using store_api.Core.DTOs.Responses;
using store_api.Core.Interfaces.Services;
using System;

namespace store_api.Controllers
{
    [Route("api/v1/shop")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrder _orderService;

        public OrderController(ILogger<OrderController> logger, IOrder orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [Route("order/initiate")]
        [HttpPost]
        public IActionResult InitiateOrder([FromBody] InitiateOrderRequest request)
        {
            var validate = Validate(request);
            if (validate != null) return validate;

            try
            {
                var response = _orderService.InitiateOrder(request);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }

        [Route("order/complete")]
        [Authorize]
        [HttpPost]
        public IActionResult CompleteOrder([FromBody] CompleteOrderRequest request)
        {
            var validate = Validate(request);
            if (validate != null) return validate;

            try
            {
                request.CustomerId = CustomerId();
                var response = _orderService.CompleteOrder(request);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }

        [Route("order/authenticate")]
        [HttpPost]
        public IActionResult Authenticateorder([FromBody] AuthenticateOrderRequest request)
        {
            var validate = Validate(request);
            if (validate != null) return validate;

            try
            {
                var response = _orderService.AuthenticateOrder(request);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }

    }
}
