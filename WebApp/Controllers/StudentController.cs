using ApplicationCore.IService;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApp.Models.ViewModel;
using WebApp.Models.InputModel;
using ApplicationCore.Service;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class StudentController : BaseController
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly UserManager<User> UserManager;
        private readonly IMapper Mapper;

        public StudentController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager, IMapper mapper) : base(userManager)
        {
            StudentService = studentService;
            CourseService = courseService;
            UserManager = userManager;
            Mapper = mapper;
        }

        [Authorize(Roles = Role.Admin)]
        [Route("List")]
        public async Task<IActionResult> AllStudents()
        {
            var students = await StudentService.GetAllStudentsAsync();
            return View(Mapper.Map<IReadOnlyCollection<StudentInfoViewModel>>(students));
        }

        [Authorize(Roles = Role.Student)]
        public IActionResult Profile()
        {
            if (CurrentUser == null)
            {
                return NotFound();
            }

            return View(Mapper.Map<ProfileViewModel>(CurrentUser));
        }

        [Authorize(Roles = $"{Role.Student}, {Role.Admin}")]
        [Route("Marks")]
        public async Task<IActionResult> MarksFromCourse([FromQuery]int? studentId, [FromQuery]int courseId)
        {
            Student? student = null;
            if (User.IsInRole(Role.Admin))
            {
                if (studentId == null)
                {
                    return NotFound();
                }

                student = await StudentService.GetStudent(studentId.Value);
            }else
            {
                if (CurrentUser == null)
                {
                    return NotFound();
                }

                student = await StudentService.GetStudentByUserId(CurrentUser.Id);
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
                CourseModuleMarks = courseModules.Select( cm =>
                    new MarkViewModel
                    {
                        CourseModuleId = cm.CourseModuleId,
                        Mark = marks.FirstOrDefault(m => m.CourseModuleId == cm.CourseModuleId)?.Mark
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GiveMark([FromForm]GiveMarkModel model)
        {
            var student = await StudentService.GetStudent(model.StudentId);
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
            await StudentService.SaveStudent(student);

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
        public async Task<IActionResult> RegisterStudent([FromForm] RegisterStudentModel student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            var user = CreateUser();

            user.Email = student.Email;
            user.UserName = student.Email;

            await UserManager.CreateAsync(user, student.Password);

            await UserManager.AddToRoleAsync(user, Role.Student);

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