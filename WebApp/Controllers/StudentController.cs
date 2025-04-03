using ApplicationCore.IService;
using AutoMapper;
using Domain.Models;
using Domain.Models.Aggregate;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extension;
using WebApp.externalServices;
using WebApp.Models;
using WebApp.Models.InputModel;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class StudentController : BaseController
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly UserManager<User> UserManager;
        private readonly IPasswordGenerator PasswordGenerator;
        private readonly IEmailSender EmailSender;
        private readonly IMapper Mapper;

        public StudentController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager, IMapper mapper, IPasswordGenerator passwordGenerator, IEmailSender emailSender)
        {
            StudentService = studentService;
            CourseService = courseService;
            UserManager = userManager;
            PasswordGenerator = passwordGenerator;
            EmailSender = emailSender;
            Mapper = mapper;
        }

        [Authorize(Roles = Role.Admin)]
        [Route("List")]
        public async Task<IActionResult> AllStudentsAsync()
        {
            var students = await StudentService.GetAllStudentsAsync();
            return View(Mapper.Map<IReadOnlyCollection<StudentInfoViewModel>>(students));
        }

        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> ProfileAsync()
        {
            var userId = UserManager.GetUserId(User);
            var email = HttpContext.Session.GetEmail();

            if (string.IsNullOrEmpty(userId) || email == null)
            {
                return NotFound();
            }

            var student = await StudentService.GetStudentByUserIdAsync(userId);
            if (student == null)
            {
                return NotFound();
            }
            var model = new ProfileViewModel
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                Email = email
            };
            
            return View(model);
        }

        [Authorize(Roles = $"{Role.Student}, {Role.Admin}")]
        [Route("Marks")]
        public async Task<IActionResult> MarksFromCourseAsync([FromQuery] int? studentId, [FromQuery] int courseId)
        {
            Student? student = null;
            if (User.IsInRole(Role.Admin))
            {
                if (studentId == null)
                {
                    return NotFound();
                }

                student = await StudentService.GetStudentAsync(studentId.Value);
            }
            else
            {
                var userId = UserManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound();
                }

                student = await StudentService.GetStudentByUserIdAsync(userId);
            }

            if (student == null)
            {
                return NotFound();
            }

            var marks = await StudentService.GetStudentMarksForCourseAsync(student.StudentId, courseId);
            var course = await CourseService.GetCourseAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            var courseModules = course.CourseModules;


            var model = new MarkedModulesViewModel
            {
                StudentId = student.StudentId,
                CourseId = courseId,
                CourseModuleMarks = courseModules.Select(cm =>
                    new MarkViewModel
                    {
                        CourseModuleId = cm.CourseModuleId,
                        Mark = marks.FirstOrDefault(m => m.CourseModuleId == cm.CourseModuleId)?.Mark,
                        CourseDescription = cm.Description
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GiveMarkAsync([FromForm] GiveMarkModel model)
        {
            var student = await StudentService.GetStudentAsync(model.StudentId);
            var course = await CourseService.GetCourseAsync(model.CourseId);

            if (student == null || course == null)
            {
                return NotFound();
            }

            var courseModule = course.CourseModules.FirstOrDefault(cm => cm.CourseModuleId == model.CourseModuleId);

            if (courseModule == null)

            {
                return NotFound();
            }

            student.GiveMark(courseModule, model.Mark);
            await StudentService.SaveStudentAsync(student);

            Console.WriteLine("mark given");
            return RedirectToAction("List", "Student");
        }

        [HttpGet]
        [Route("Register")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult RegisterStudent()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
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
            HttpContext.Session.SetIfDefaultpassword(user, true);

            var password = PasswordGenerator.GenerateRandom();

            await UserManager.CreateAsync(user, password);

            var student = new Student(model.FirstName, model.LastName, model.BirthDate, new LearningPlan(model.FirstName+ user.Id));
          
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