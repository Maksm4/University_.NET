using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync()
        {

        }

        [HttpGet]
        [Authorize(Roles = $"{Role.Admin}, {Role.Student}")]
        public async Task<IActionResult> LogoutAsync()
        {

        }
    }
}