using ApplicationCore.IService;
using Domain.Models;
using Domain.Models.Aggregate;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.externalServices;
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
        private readonly IPasswordGenerator PasswordGenerator;
        private readonly IEmailSender EmailSender;
        private readonly IStudentService StudentService;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IPasswordGenerator passwordGenerator, IEmailSender emailSender, IStudentService studentService)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            PasswordGenerator = passwordGenerator;
            EmailSender = emailSender;
            StudentService = studentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View(model);
                }

                var result = await SignInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    HttpContext.Session.SetString(SessionData.Email.ToString(), model.Email);
                    if (await UserManager.IsInRoleAsync(user, Role.Admin))
                    {
                        return RedirectToAction("List", "Student");
                    }
                    else
                    {
                        if (user.studentId == null)
                        {
                            return NotFound();
                        }
                        HttpContext.Session.SetInt32(SessionData.StudentId.ToString(), user.studentId.Value);
                        HttpContext.Session.SetString(SessionData.HasDefaultPassword.ToString(), user.HasDefaultpassword.ToString());

                        if (user.HasDefaultpassword)
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
        public async Task<IActionResult> LogoutAsync()
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
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = UserManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
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
                    user.HasDefaultpassword = false;
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
        [Authorize(Roles = Role.Admin)]
        public IActionResult RegisterStudent()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RegisterStudentAsync([FromForm] RegisterStudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = CreateUser();

            user.Email = model.Email;
            user.UserName = model.Email;
            user.HasDefaultpassword = true;

            var password = PasswordGenerator.GenerateRandom();

            await UserManager.CreateAsync(user, password);

            var student = new Student(model.FirstName, model.LastName, model.BirthDate, new LearningPlan(model.FirstName + user.Id));

            user.student = student;
            await StudentService.SaveStudentAsync(student);

            await UserManager.AddToRoleAsync(user, Role.Student);

            EmailSender.SendEmail($"your temporarry password: {password}", user.Email);
            return RedirectToAction("List", "Student");
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}");
            }
        }
    }
}