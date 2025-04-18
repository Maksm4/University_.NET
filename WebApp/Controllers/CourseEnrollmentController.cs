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
        [Authorize(Roles = Role.Student)]
        [Route("list")]
        public async Task<IActionResult> GetStudentEnrolledCoursesAsync()
        {
            int? studentId = HttpContext.Session.GetStudentId();

            if (!studentId.HasValue)
            {
                return Forbid();
            }

            var enrolledCourses = await studentService.GetEnrolledCoursesWithGradesAsync(studentId.Value);

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


        [HttpGet("studentId")]
        [Authorize(Roles = Role.Admin)]
        [Route("adminList")]
        public async Task<IActionResult> GetStudentEnrolledCoursesAsync([FromQuery] int studentId)
        {
            if (studentId < 0)
            {
                return BadRequest();
            }

            var enrolledCourses = await studentService.GetEnrolledCoursesWithGradesAsync(studentId);

            var model = enrolledCourses.Where(enrolledCrs => enrolledCrs != null).Select(enrolledCrs =>
            new CourseEnrolledViewModel
            {
                CourseId = enrolledCrs!.CourseId,
                Name = enrolledCrs.CourseName,
                Description = enrolledCrs.CourseDescription,
                IsActive = enrolledCrs.IsActive,
                DateTimeRange = new DateTimeRange(enrolledCrs.DateTimeRange.StartTime, enrolledCrs.DateTimeRange.EndTime),
                StudentId = studentId
            });

            return View(model);
        }


        [HttpPost("courseId")]
        [Authorize(Roles = Role.Student)]
        [Route("enroll")]
        public async Task<IActionResult> CourseEnrollAsync([FromQuery] int courseId)
        {
            var studentId = HttpContext.Session.GetStudentId();

            if (!studentId.HasValue)
            {
                return Forbid();
            }

            if (courseId < 0)
            {
                return BadRequest();
            }

            await studentService.EnrollStudentInCourseAsync(studentId.Value, courseId);
            return RedirectToAction("List");
        }
    }
}