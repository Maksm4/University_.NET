using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Extension;

namespace WebApp.Attributes
{
    public class RequirePasswordChangeAttribute : Attribute, IAuthorizationFilter
    {
        private const string ChangePasswordAction = "/Account/ChangePassword";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User != null &&
                context.HttpContext.User.Identity != null
                && context.HttpContext.User.Identity.IsAuthenticated)
            {
                var hasDefaultPassword = context.HttpContext.Session.HasDefaultPassword();
                var path = context.HttpContext.Request.Path;

                if (hasDefaultPassword && !path.Equals(ChangePasswordAction))
                {
                    context.Result = new RedirectToActionResult("ChangePassword", "Account", null);
                }
            }
        }
    }
}