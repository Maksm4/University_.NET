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

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        [Route("list")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await studentService.GetAllStudentsAsync();
            return View(mapper.Map<IReadOnlyCollection<StudentInfoViewModel>>(students));
        }

        [HttpGet]
        [Authorize(Roles = Role.Student)]
        [Route("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            string? email = HttpContext.Session.GetEmail();
            int? studentId = HttpContext.Session.GetStudentId();

            if (!studentId.HasValue)
            {
                return Forbid();
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
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

        [HttpGet]
        [Authorize(Roles = Role.Student)]
        [Route("marks")]
        public async Task<IActionResult> GetMarksFromCourseAsync([FromQuery] int courseId)
        {
            int? studentId = HttpContext.Session.GetStudentId();

            if(!studentId.HasValue)
            {
                return Forbid();
            }

            if (courseId < 0)
            {
                return BadRequest();
            }

            return View(await GetStudentCourseMarkedModulesAsync(studentId.Value, courseId));
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        [Route("adminMarks")]
        public async Task<IActionResult> GetMarksFromCourseAsync([FromQuery] int studentId, [FromQuery] int courseId)
        {
            if (studentId < 0 || courseId < 0)
            {
                return BadRequest();
            }

            return View(await GetStudentCourseMarkedModulesAsync(studentId, courseId));
        }
        private async Task<MarkedModulesViewModel> GetStudentCourseMarkedModulesAsync(int studentId, int courseId)
        {
            var courseModules = await courseService.GetCourseModulesWithMarkAsync(studentId, courseId);

            return new MarkedModulesViewModel
            {
                StudentId = studentId,
                CourseId = courseId,
                CourseModuleMarks = mapper.Map<List<MarkViewModel>>(courseModules)
            };
        }

        [HttpPost]
        [Route("giveMark")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GiveMarkAsync([FromForm] GiveMarkModel model)
        {
            if (model.CourseId < 0 || model.CourseModuleId < 0 || model.StudentId < 0)
            {
                return BadRequest();
            }

            if(await studentService.GiveMarkForCourseModuleAsync(model.StudentId, model.CourseId, model.CourseModuleId, model.Mark))
            {
                return RedirectToAction("list", "Student");
            }
            return View(model);
        }
    }
}