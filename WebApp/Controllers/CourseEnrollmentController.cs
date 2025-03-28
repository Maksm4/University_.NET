using ApplicationCore.IService;
using AutoMapper;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class CourseEnrollmentController : Controller
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
        public async Task<IActionResult> StudentEnrolledCourses()
        {
            var userId = UserManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound();
            }

            var student = await StudentService.GetStudentByUserId(userId);

            if (student == null)
            {
                return NotFound();
            }

            var courses = await StudentService.GetCoursesTakenByStudentAsync(student.StudentId);
            return View(Mapper.Map<IReadOnlyCollection<CourseViewModel>>(courses));
        }

        [HttpGet("courseId")]
        [Route("Enroll")]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> CourseEnroll(int courseId)
        {
            //repository extract
            var currentUser = await UserManager.Users
                .Include(u => u.student)
                .FirstOrDefaultAsync(u => u.Id == UserManager.GetUserId(User));

            if (currentUser == null || currentUser.student == null)
            {
                return NotFound();
            }
            var course = await CourseService.GetCourseAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            //check if already enrolled
            var coursesTaken = await StudentService.GetCoursesTakenByStudentAsync(currentUser.student.StudentId);
            if (!coursesTaken.Contains(course))
            {
                currentUser.student.EnrollInCourse(course);
                await StudentService.SaveStudent(currentUser.student);
            }
            return RedirectToAction("List");
        }
    }
}