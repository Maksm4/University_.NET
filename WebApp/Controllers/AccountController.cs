using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Extension;
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
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View(model);
                }

                var result = await SignInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ExtensionClaimTypes.UserId.ToString(), user.Id)
                    };

                    if (await UserManager.IsInRoleAsync(user, Role.Admin))
                    {
                        await SignInManager.SignInWithClaimsAsync(user, model.RememberMe, claims);
                        return RedirectToAction("List", "Student");
                    }
                    else
                    {
                        if (user.studentId == null)
                        {
                            return NotFound();
                        }

                        claims.AddRange(new List<Claim>()
                        {
                            new Claim(ExtensionClaimTypes.StudentId.ToString(), user.studentId.Value.ToString()),
                            new Claim(ExtensionClaimTypes.DefaultPassword.ToString(), user.defaultpassword.ToString())
                        });

                        await SignInManager.SignInWithClaimsAsync(user, model.RememberMe, claims);

                        if (user.defaultpassword)
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
                var userId = User.GetUserId();
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = await UserManager.FindByIdAsync(userId);
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
    }
}