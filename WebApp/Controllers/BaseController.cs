using Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static User? CurrentUser { get; set; }
        //protected User CurrentUser
        //{
        //    get
        //    {
        //        if (_currentUser == null)
        //        {
        //            var userJson = HttpContext.Session.GetString("CurrentUser");
        //            if (!string.IsNullOrEmpty(userJson))
        //            {
        //                _currentUser = JsonConvert.DeserializeObject<User>(userJson);
        //            }
        //        }
        //        return _currentUser;
        //    }
        //}
        public BaseController() { }
    }
}