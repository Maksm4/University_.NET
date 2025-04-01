using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected User? CurrentUser { get; private set; }

        private readonly UserManager<User> UserManager;

        public BaseController(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(Role.Admin))
                {
                    CurrentUser = UserManager.GetUserAsync(User).Result;
                }else if (User.IsInRole(Role.Student))
                {
                    CurrentUser = UserManager.Users.Include(u => u.student)
                        .FirstOrDefault(u => u.Id == UserManager.GetUserId(User));
                }
            }
        }
    }
}
