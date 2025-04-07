using ApplicationCore.IService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Extension;
using WebApp.Models;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class CourseController : Controller
    {
        private readonly ICourseService CourseService;
        private readonly IMapper Mapper;
        public CourseController(ICourseService courseService, IMapper mapper) 
        {
            CourseService = courseService;
            Mapper = mapper;
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
            
            if (studentId == null)
            {
                return NotFound();
            }

            var courses = await CourseService.GetAllCoursesWithEnrollmentStatusAsync(studentId.Value);
            return View(Mapper.Map<IReadOnlyCollection<CourseViewModel>>(courses));
        }
    }
}