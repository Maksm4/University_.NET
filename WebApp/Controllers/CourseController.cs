using ApplicationCore.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extension;
using WebApp.Models;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class CourseController : BaseController
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        public CourseController(IStudentService studentService, ICourseService courseService) 
        {
            StudentService = studentService;
            CourseService = courseService;
        }

        [Route("ListAdmin")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AllCoursesAdminAsync()
        {
            var courses = await CourseService.GetCoursesAsync();

            var model = courses.Select(
                c =>
            new BaseCourseViewModel
            {
                CourseId = c.CourseId,
                Name = c.Name,
                Description = c.Description,
                IsActive = !c.IsDeprecated
            });

            return View(model);
        }

        [Route("List")]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> AllCoursesStudentAsync()
        {
            var studentId = HttpContext.Session.GetStudentId();
            var courses = await CourseService.GetCoursesAsync();
            
            if (studentId == null)
            {
                return NotFound();
            }

            var studentCourses = await StudentService.GetEnrolledCoursesAsync(studentId.Value);
            return View(courses.Select(c =>
            new CourseViewModel
            {
                CourseId = c.CourseId,
                Name = c.Name,
                Description = c.Description,
                IsActive = !c.IsDeprecated,
                isEnrolled = studentCourses.Any(sc => sc.CourseId == c.CourseId)
            }));
        }
    }
}