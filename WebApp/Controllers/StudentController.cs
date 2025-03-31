using ApplicationCore.IService;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class StudentController : Controller
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly UserManager<User> UserManager;
        private readonly IMapper Mapper;

        public StudentController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager, IMapper mapper)
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
        public async Task<IActionResult> Profile()
        {
            //extract to repiository
            var user = await UserManager.Users
                .Include(u => u.student)
                .FirstOrDefaultAsync(u => u.Id == UserManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }

            return View(Mapper.Map<ProfileViewModel>(user));
        }

        [Authorize(Roles = $"{Role.Student}, {Role.Admin}")]
        [Route("Marks")]
        public async Task<IActionResult> MarksFromCourse(int? studentId, int courseId)
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
                var userId = UserManager.GetUserId(User);

                if (userId == null)
                {
                    return NotFound();
                }

                student = await StudentService.GetStudentByUserId(userId);
            }

            if (student == null)
            {
                return NotFound();
            }

            var marks = await StudentService.GetStudentMarksForCourseAsync(student.StudentId, courseId);


            var model = new MarkedModulesViewModel
            {
                StudentId = student.StudentId,
                CourseId = courseId,
                CourseModuleMarks = marks.GroupBy(m => m.CourseModuleId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(m => m.Mark).ToList()
                    )
            };

            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GiveMark(int studentId, int courseId, int courseModuleId, int mark)
        {
            var student = await StudentService.GetStudent(studentId);
            var course = await CourseService.GetCourseAsync(courseId);

            if (student == null || course == null)
            {
                return NotFound();
            }

            var courseModule = course.CourseModules.FirstOrDefault(cm => cm.CourseModuleId == courseModuleId);

            if (courseModule == null)

            {
                return NotFound();
            }

            student.GiveMark(courseModule, mark);
            await StudentService.SaveStudent(student);

            Console.WriteLine("mark given");
            return RedirectToAction("List", "Student");
        }
    }
}