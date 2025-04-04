using ApplicationCore.IService;
using Domain.Models.VObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extension;
using WebApp.Models;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class CourseEnrollmentController : BaseController
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        public CourseEnrollmentController(IStudentService studentService, ICourseService courseService) 
        {
            StudentService = studentService;
            CourseService = courseService;
        }

        [HttpGet]
        [Route("List")]
        [Authorize(Roles = $"{Role.Admin}, {Role.Student}")]
        public async Task<IActionResult> StudentEnrolledCoursesAsync([FromQuery] int? studentId)
        {
            if (!User.IsInRole(Role.Admin))
            {
                studentId = HttpContext.Session.GetStudentId();
            }

            if (studentId == null)
            {
                return NotFound();
            }

            var enrolledCourses = await StudentService.GetEnrolledCoursesAsync(studentId.Value);
            var courses = await StudentService.GetCoursesTakenByStudentAsync(studentId.Value);

            var viewModel = enrolledCourses.Select(ec =>
            new CourseEnrolledViewModel
            {
                CourseId = ec.CourseId,
                Name = courses.First(c => c.CourseId == ec.CourseId).Name,
                Description = courses.First(c => c.CourseId == ec.CourseId).Description,
                IsActive = !courses.First(c => c.CourseId == ec.CourseId).IsDeprecated,
                DateTimeRange = new DateTimeRange(ec.DateTimeRange.StartTime, ec.DateTimeRange.EndTime),
                StudentId = studentId.Value
            });

            return View(viewModel);
        }

        [HttpGet("courseId")]
        [Route("Enroll")]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> CourseEnrollAsync([FromQuery] int courseId)
        {
            var studentId = HttpContext.Session.GetStudentId();
            if (studentId == null)
            {
                return NotFound();
            }
            await StudentService.EnrollStudentInCourseAsync(studentId.Value, courseId);
            return RedirectToAction("List");
        }
    }
}