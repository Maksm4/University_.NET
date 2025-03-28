using Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected User currentUser { get; }
    }
}
