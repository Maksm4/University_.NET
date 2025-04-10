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
        private readonly ICourseService courseService;
        private readonly IMapper mapper;
        public CourseController(ICourseService courseService, IMapper mapper) 
        {
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        [Route("listAdmin")]
        public async Task<IActionResult> GetAllCoursesAdminAsync()
        {
            var courses = await courseService.GetCoursesAsync();

            var model = courses.Select(
                crs =>
            new BaseCourseViewModel
            {
                CourseId = crs.CourseId,
                Name = crs.Name,
                Description = crs.Description,
                IsActive = !crs.IsDeprecated
            });

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Role.Student)]
        [Route("list")]
        public async Task<IActionResult> GetAllCoursesStudentAsync()
        {
            var studentId = HttpContext.Session.GetStudentId();
            
            if (!studentId.HasValue)
            {
                return Forbid();
            }

            var courses = await courseService.GetAllCoursesWithEnrollmentStatusAsync(studentId.Value);
            return View(mapper.Map<IReadOnlyCollection<CourseViewModel>>(courses));
        }
    }
}