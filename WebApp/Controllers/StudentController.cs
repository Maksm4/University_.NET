using ApplicationCore.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extension;
using WebApp.Models;
using WebApp.Models.InputModel;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class StudentController : Controller
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly IMapper Mapper;

        public StudentController(IStudentService studentService, ICourseService courseService, IMapper mapper)
        {
            StudentService = studentService;
            CourseService = courseService;
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
            var email = HttpContext.Session.GetEmail();
            var studentId = HttpContext.Session.GetStudentId();

            if (email == null || studentId == null)
            {
                return NotFound();
            }

            var student = await StudentService.GetStudentAsync(studentId.Value);
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
            if (!User.IsInRole(Role.Admin))
            {
                studentId = HttpContext.Session.GetStudentId();
            }

            if (studentId == null)
            {
                return NotFound();
            }

            var courseModules = await CourseService.GetCourseModulesWithMark(studentId.Value, courseId);

            return View(new MarkedModulesViewModel
            {
                StudentId = studentId.Value,
                CourseId = courseId,
                CourseModuleMarks = Mapper.Map<List<MarkViewModel>>(courseModules)
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GiveMarkAsync([FromForm] GiveMarkModel model)
        {
            if(await StudentService.GiveMarkForCourseModuleAsync(model.StudentId, model.CourseId, model.CourseModuleId, model.Mark))
            {
                return RedirectToAction("List", "Student");
            }
            return View(model);
        }
    }
}