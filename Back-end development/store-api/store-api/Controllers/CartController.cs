using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using store_api.Core.DTOs.Requests;
using store_api.Core.DTOs.Responses;
using store_api.Core.Interfaces.Services;
using System;

namespace store_api.Controllers
{
    [Route("api/v1/shop")]
    [ApiController]
    public class CartController : BaseController
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICart _service;

        public CartController(ILogger<CartController> logger, ICart service)
        {
            _logger = logger;
            _service = service;
        }

        [Route("cart/add")]
        [HttpPost]
        public IActionResult AddToCart(AddToCartRequest request)
        {
            var validate = Validate(request);
            if (validate != null) return validate;

            try
            {
                var response = _service.AddToCart(request);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }

        [Route("cart/add/bulk")]
        [HttpPost]
        public IActionResult AddToCart(AddBulkCartRequest request)
        {
            var validate = Validate(request);
            if (validate != null) return validate;

            try
            {
                var response = _service.AddTBulkToCart(request);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }

        [Route("cart/remove")]
        [HttpPost]
        public IActionResult RemoveFromCart(RemoveFromCartRequest request)
        {
            var validate = Validate(request);
            if (validate != null) return validate;

            try
            {
                var response = _service.RemoveFromCart(request);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }

        [Route("cart/save")]
        [HttpPost]
        public IActionResult SaveCartForLater(SaveForLaterRequest request)
        {
            var validate = Validate(request);
            if (validate != null) return validate;

            try
            {
                var response = _service.SaveCartForLater(request);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }


        [Route("cart/view/{sessionid}")]
        [HttpGet]
        public IActionResult ViewCart(string sessionid)
        {
            try
            {
                var response = _service.ViewCart(sessionid);
                return new ObjectResult(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new FailureResponse("99", ex.Message));
            }
        }

    }
}
