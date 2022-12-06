using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using store_api.Core.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace store_api.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(params string[] claim) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { claim };
        }
    }
    public class AuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] _claim;
        readonly IUnitOfWork _work;
        public AuthorizeFilter(IUnitOfWork work, params string[] claim)
        {
            _claim = claim;
            _work = work;
        }
        private bool IsAcountLocked(DateTime? lockedUntil)
        {
            if (lockedUntil != null)
            {
                if (lockedUntil.Value > DateTime.UtcNow)
                {
                    var seconds = (lockedUntil.Value - DateTime.UtcNow).TotalSeconds;
                    return true;
                }
                return false;
            }
            return false;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;
            try
            {
                var token = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
                var tokens = token.Value.ToString().Replace("Bearer", "").Replace("bearer", "").Replace("BEARER", "").Replace(" ", "").Trim();
                var foundtoken = _work.AccessTokenRepository.FindToken(tokens);
                if (foundtoken == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "sorry, session is no longer active", status_code = "05" });
                    return;
                }
                //check the customer
                var user = _work.CustomerRepository.Find(Convert.ToInt64(context.HttpContext.User.Identity.Name));
                if (user == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "sorry, session timed out", status_code = "05" });
                    return;
                }
                if (IsAcountLocked(user.AccountLockedUntil))
                {
                    var seconds = (user.AccountLockedUntil.Value - DateTime.UtcNow).TotalSeconds;
                    context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "Your account is currently locked, please reach out to admin", status_code = "05" });
                    return;
                }
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "session timeout", status_code = "05" });
                return;
            }
            if (IsAuthenticated)
            {
                if (_claim != null && _claim.Length > 0)
                {
                    bool flagClaim = false;
                    var userClaims = context.HttpContext.User.Claims;
                    var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                    if (roleClaim == null)
                    {
                        context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "No role set for user", status_code = "05" });
                    }
                    else if (roleClaim.Value == "" || string.IsNullOrEmpty(roleClaim.Value))
                    {
                        context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "No role was found for authenticated user", status_code = "06" });
                    }
                    else
                    {
                        //convert to array
                        var userroles = roleClaim.Value.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in _claim)
                        {
                            foreach (var item2 in userroles)
                            {
                                if (item.Equals(item2, System.StringComparison.InvariantCultureIgnoreCase))
                                {
                                    flagClaim = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!flagClaim)
                        context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "You are not authorized to access this resource" });
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult(new { status = "fail", message = "You need to be authenticated to access this resource" });
            }
            return;
        }
    }
    public static class PermissionExtension
    {
        public static bool HavePermission(this Controller c, string claimValue)
        {
            var user = c.HttpContext.User as ClaimsPrincipal;
            bool havePer = user.HasClaim(claimValue, claimValue);
            return havePer;
        }
        public static bool HavePermission(this IIdentity claims, string claimValue)
        {
            var userClaims = claims as ClaimsIdentity;
            bool havePer = userClaims.HasClaim(claimValue, claimValue);
            return havePer;
        }
    }
}
