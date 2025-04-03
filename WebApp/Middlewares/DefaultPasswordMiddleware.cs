using Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WebApp.Extension;
using WebApp.Models;

namespace WebApp.Middlewares
{
    public class DefaultPasswordMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ChangePasswordAction = "/Account/ChangePassword";

        public DefaultPasswordMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User != null 
                && httpContext.User.Identity != null
                && httpContext.User.Identity.IsAuthenticated
                && httpContext.User.IsInRole(Role.Student))
            {
                var hasDefaultPassword = httpContext.Session.HasDefaultPassword();
                var path = httpContext.Request.Path.Value ?? throw new Exception();

                if(hasDefaultPassword && !path.Equals(ChangePasswordAction))
                {
                    httpContext.Response.Redirect(ChangePasswordAction);
                }
            }
            
            await _next(httpContext);
        }
    }

    public static class DefaultPasswordMiddlewareExtensions
    {
        public static IApplicationBuilder UseDefaultPasswordMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DefaultPasswordMiddleware>();
        }
    }
}
