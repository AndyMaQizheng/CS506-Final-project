using Microsoft.AspNetCore.Mvc;
using store_api.Core.DTOs.Responses;
using store_api.Core.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace store_api.Controllers
{
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Validate(dynamic request)
        {
            if (request == null)
            {
                return BadRequest(new Response { StatusCode = "01", Status = "fail", Message = "Null object detected!" });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                var error = errors.Select(x => x.Select(y => y.ErrorMessage)).ToList();
                return BadRequest(new Response { StatusCode = "01", Status = "fail", Message = error.First().First() });
            }
            return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ValidateLong(string request)
        {
            if (string.IsNullOrEmpty(request))
            {
                return BadRequest(new Response { StatusCode = "01", Status = "fail", Message = "Null object detected!" });
            }
            if (!Regex.Match(request, "^[0-9]+$").Success)
            {
                return BadRequest(new Response { StatusCode = "01", Status = "fail", Message = "Invalid data supplied" });
            }
            return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetIPAddress()
        {
            string ipAddress = HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"].ToString();

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return HttpContext.Request.Headers["REMOTE_ADDR"].ToString();
        }

        [ApiExplorerSettings(IgnoreApi = true)]

        public long CustomerId()
        {
            try
            {
                return Convert.ToInt64(User.Identity.Name);
            }
            catch (Exception)
            {
                throw new CustomException("Unable to complete");
            }
        }

    }
}
