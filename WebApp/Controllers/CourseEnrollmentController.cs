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
        private readonly IStudentService studentService;
        public CourseEnrollmentController(IStudentService studentService) 
        {
            this.studentService = studentService;
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

            var enrolledCourses = await studentService.GetEnrolledCoursesAsync(studentId.Value);

            var model = enrolledCourses.Where(enrolledCrs => enrolledCrs != null).Select(enrolledCrs =>
            new CourseEnrolledViewModel
            {
                CourseId = enrolledCrs!.CourseId,
                Name = enrolledCrs.CourseName,
                Description = enrolledCrs.CourseDescription,
                IsActive = enrolledCrs.IsActive,
                DateTimeRange = new DateTimeRange(enrolledCrs.DateTimeRange.StartTime, enrolledCrs.DateTimeRange.EndTime),
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
            await studentService.EnrollStudentInCourseAsync(studentId.Value, courseId);
            return RedirectToAction("List");
        }
    }
}