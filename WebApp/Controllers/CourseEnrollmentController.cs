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
    public class CourseEnrollmentController : Controller
    {
        private readonly IStudentService StudentService;
        public CourseEnrollmentController(IStudentService studentService) 
        {
            StudentService = studentService;
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

            var model = enrolledCourses.Where(ec => ec != null).Select(ec =>
            new CourseEnrolledViewModel
            {
                CourseId = ec!.CourseId,
                Name = ec.CourseName,
                Description = ec.CourseDescription,
                IsActive = ec.IsActive,
                DateTimeRange = new DateTimeRange(ec.DateTimeRange.StartTime, ec.DateTimeRange.EndTime),
                StudentId = studentId.Value
            });

            return View(model);
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