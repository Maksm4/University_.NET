using Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static User? CurrentUser { get; set; }

        public BaseController() { }
    }
}