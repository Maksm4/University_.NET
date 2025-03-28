using ApplicationCore.IService;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly UserManager<User> UserManager;
        public EnrollmentController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager)
        {
            StudentService = studentService;
            CourseService = courseService;
            UserManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Student")]
        public async Task<IActionResult> List(int studentId)
        {
            var courses = await StudentService.GetCoursesTakenByStudentAsync(studentId);
            return View(courses);
        }
    }
}