using ApplicationCore.IService;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly UserManager<User> UserManager;

        public StudentController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager)
        {
            this.StudentService = studentService;
            this.CourseService = courseService;
            this.UserManager = userManager;
        }


        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            var students = await StudentService.GetAllStudentsAsync();
            return View(students);
        }


        public async Task<IActionResult> Profile()
        {
            var user = await UserManager.Users
                .Include(u => u.student)
                .FirstOrDefaultAsync(u => u.Id == UserManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }
           
            return View(user);
        }

        public IActionResult Add(Student student)
        {
            return View(student);
        }
    }
}