using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Models.InputModel;
using WebApp.Models.ViewModel;

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
                        if(CurrentUser == null)
                        {
                            return NotFound();
                        }

                        if (CurrentUser.defaultpassword)
                        {
                            return RedirectToAction("ChangePassword", "Account");
                        }else
                        {
                            return RedirectToAction("Profile", "Student");
                        }
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
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize(Roles = Role.Student)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (CurrentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                //to be safe reterive user from db 
                var user = await UserManager.FindByIdAsync(CurrentUser.Id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    user.defaultpassword = false;
                    await UserManager.UpdateAsync(user);
                    await SignInManager.SignOutAsync();

                    return RedirectToAction("Login", "Account");
                }else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SuccessfullPasswordChange()
        {
            return View();
        }


        private void SetCurrentUser()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                //User? user;
                if (User.IsInRole(Role.Admin))
                {
                    
                    CurrentUser = UserManager.GetUserAsync(User).Result;
                    //HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(user));
                }
                else if (User.IsInRole(Role.Student))
                {
                    //need student data for profile info
                    CurrentUser = UserManager.Users.Include(u => u.student)
                        .FirstOrDefault(u => u.Id == UserManager.GetUserId(User));
                    //HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(user));
                }
            }
        }
    }
}