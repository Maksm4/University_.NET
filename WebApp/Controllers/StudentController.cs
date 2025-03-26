using ApplicationCore.IService;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;

        public StudentController(IStudentService studentService, ICourseService courseService)
        {
            this.studentService = studentService;
            this.courseService = courseService;
        }


        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            var students = await studentService.GetAllStudentsAsync();
            return View(students);
        }


        public IActionResult Profile()
        {
            var userName = User.Identity?.Name;

            if (userName == null)
            {
                return View();
            }
            //var student = studentService.GetStudent();

            return View();
        }

        public IActionResult Add(Student student)
        {
            return View(student);
        }
    }
}