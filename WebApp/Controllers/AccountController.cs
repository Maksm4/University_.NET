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
    [Route("[controller]")]
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
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
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
                        return RedirectToAction("list", "Student");
                    }
                    else
                    {
                        if (!user.studentId.HasValue)
                        {
                            return Forbid();
                        }
                        HttpContext.Session.SetInt32(SessionData.StudentId.ToString(), user.studentId.Value);
                        HttpContext.Session.SetString(SessionData.HasDefaultPassword.ToString(), user.HasDefaultpassword.ToString());

                        if (user.HasDefaultpassword)
                        {
                            return RedirectToAction("changePassword", "Account");
                        }else
                        {
                            return RedirectToAction("profile", "Student");
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
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "Account");
        }

        [HttpGet]
        [Authorize(Roles = Role.Student)]
        [Route("changePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("changePassword")]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("login", "Account");
                }

                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest();
                }

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    user.HasDefaultpassword = false;
                    await userManager.UpdateAsync(user);
                    await signInManager.SignOutAsync();

                    return RedirectToAction("login", "Account");
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
        [Route("registerStudent")]
        public IActionResult RegisterStudent()
        {
            return View();
        }

        [HttpPost]
        [Route("registerStudent")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> RegisterStudentAsync([FromForm] RegisterStudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            { 
                Email = model.Email,
                UserName = model.Email,
                HasDefaultpassword = true
            };

            var password = passwordGenerator.GenerateRandom();

            await userManager.CreateAsync(user, password);
            var student = new Student(model.FirstName, model.LastName, model.BirthDate, new LearningPlan($"{model.FirstName}{user.Id}"));

            user.student = student;
            await studentService.SaveStudentAsync(student);

            await userManager.AddToRoleAsync(user, Role.Student);

            emailSender.SendEmail($"your temporarry password: {password}", user.Email);
            return RedirectToAction("list", "Student");
        }
    }
}