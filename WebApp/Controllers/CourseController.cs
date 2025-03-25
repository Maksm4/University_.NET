using ApplicationCore.IService;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;
        public CourseController(IStudentService studentService, ICourseService courseService)
        {
            this.studentService = studentService;
            this.courseService = courseService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Taken()
        {
            return View();
        }
        
        public IActionResult Enroll(int courseId)
        {

        }

    }
}