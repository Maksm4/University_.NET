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
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;
        private readonly IMapper mapper;

        public StudentController(IStudentService studentService, ICourseService courseService, IMapper mapper)
        {
            this.studentService = studentService;
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [Authorize(Roles = Role.Admin)]
        [Route("List")]
        public async Task<IActionResult> AllStudentsAsync()
        {
            var students = await studentService.GetAllStudentsAsync();
            return View(mapper.Map<IReadOnlyCollection<StudentInfoViewModel>>(students));
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

            var student = await studentService.GetStudentAsync(studentId.Value);
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

            var courseModules = await courseService.GetCourseModulesWithMark(studentId.Value, courseId);

            return View(new MarkedModulesViewModel
            {
                StudentId = studentId.Value,
                CourseId = courseId,
                CourseModuleMarks = mapper.Map<List<MarkViewModel>>(courseModules)
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GiveMarkAsync([FromForm] GiveMarkModel model)
        {
            if(await studentService.GiveMarkForCourseModuleAsync(model.StudentId, model.CourseId, model.CourseModuleId, model.Mark))
            {
                return RedirectToAction("List", "Student");
            }
            return View(model);
        }
    }
}