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
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IEmailSender emailSender;
        private readonly IStudentService studentService;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IPasswordGenerator passwordGenerator, IEmailSender emailSender, IStudentService studentService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.passwordGenerator = passwordGenerator;
            this.emailSender = emailSender;
            this.studentService = studentService;
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
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    HttpContext.Session.SetString(SessionData.Email.ToString(), model.Email);
                    if (await userManager.IsInRoleAsync(user, Role.Admin))
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
            await signInManager.SignOutAsync();
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
                var userId = userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    user.HasDefaultpassword = false;
                    await userManager.UpdateAsync(user);
                    await signInManager.SignOutAsync();

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

            var password = passwordGenerator.GenerateRandom();

            await userManager.CreateAsync(user, password);

            var student = new Student(model.FirstName, model.LastName, model.BirthDate, new LearningPlan($"{model.FirstName}{user.Id}"));

            user.student = student;
            await studentService.SaveStudentAsync(student);

            await userManager.AddToRoleAsync(user, Role.Student);

            emailSender.SendEmail($"your temporarry password: {password}", user.Email);
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