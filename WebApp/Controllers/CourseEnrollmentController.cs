using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Domain.Models.VObject;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly UserManager<User> UserManager;
        private readonly IMapper Mapper;
        public CourseEnrollmentController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager, IMapper mapper) 
        {
            StudentService = studentService;
            CourseService = courseService;
            UserManager = userManager;
            Mapper = mapper;
        }

        [HttpGet]
        [Route("List")]
        [Authorize(Roles = $"{Role.Admin}, {Role.Student}")]
        public async Task<IActionResult> StudentEnrolledCoursesAsync([FromQuery] int? studentId)
        {
            Student? student = null;
            if (User.IsInRole(Role.Admin))
            {
                if (studentId == null)
                {
                    return NotFound();
                }

                student = await StudentService.GetStudentAsync(studentId.Value);

            }else
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
            
            var enrolledCourses = student.GetEnrolledCourses();
            var courses = await StudentService.GetCoursesTakenByStudentAsync(student.StudentId);

            var viewModel = enrolledCourses.Select(ec =>
            new CourseEnrolledViewModel
            {
                CourseId = ec.CourseId,
                Name = courses.First(c => c.CourseId == ec.CourseId).Name,
                Description = courses.First(c => c.CourseId == ec.CourseId).Description,
                IsActive = !courses.First(c => c.CourseId == ec.CourseId).IsDeprecated,
                DateTimeRange = new DateTimeRange(ec.DateTimeRange.StartTime, ec.DateTimeRange.EndTime),
                StudentId = student.StudentId
            });

            return View(viewModel);
        }

        [HttpGet("courseId")]
        [Route("Enroll")]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> CourseEnrollAsync([FromQuery] int courseId)
        {
            var userId = UserManager.GetUserId(User);
           
            var course = await CourseService.GetCourseAsync(courseId);
            if (course == null || string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            //check if already enrolled
            var student = await StudentService.GetStudentByUserIdAsync(userId);
            if (student == null)
            {
                return NotFound();
            }

            var coursesTaken = await StudentService.GetCoursesTakenByStudentAsync(student.StudentId);
            if (!coursesTaken.Contains(course))
            {
                student.EnrollInCourse(course);
                await StudentService.SaveStudentAsync(student);
            }
            return RedirectToAction("List");
        }
    }
}