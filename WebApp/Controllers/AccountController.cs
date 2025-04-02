using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Models.InputModel;

namespace WebApp.Controllers
{
    [Controller]
    public class AccountController : BaseController
    {
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (signInResult.Succeeded)
                {
                    SetCurrentUser();

                    if (User.IsInRole(Role.Admin))
                    {
                        return RedirectToAction("List", "Student");
                    }
                    else
                    {
                        return RedirectToAction("Profile", "Student");
                    }

                }else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private void SetCurrentUser()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(Role.Admin))
                {
                    CurrentUser = UserManager.GetUserAsync(User).Result;
                }
                else if (User.IsInRole(Role.Student))
                {
                    CurrentUser = UserManager.Users.Include(u => u.student)
                        .FirstOrDefault(u => u.Id == UserManager.GetUserId(User));
                }
            }
        }
    }
}