using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Domain.Models.VObject;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper Mapper;
        public CourseEnrollmentController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager, IMapper mapper) 
        {
            StudentService = studentService;
            CourseService = courseService;
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
                if (CurrentUser == null)
                {
                    return NotFound();
                }
                student = await StudentService.GetStudentByUserIdAsync(CurrentUser.Id);
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
            if (CurrentUser == null || CurrentUser.studentId ==  null)
            {
                return NotFound();
            }
            var course = await CourseService.GetCourseAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            //check if already enrolled
            var student = await StudentService.GetStudentAsync(CurrentUser.studentId.Value);
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