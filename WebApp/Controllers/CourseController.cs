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

        [Route("ListAdmin")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AllCoursesAdminAsync()
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

        [Route("List")]
        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> AllCoursesStudentAsync()
        {
            var studentId = HttpContext.Session.GetStudentId();
            
            if (studentId == null)
            {
                return NotFound();
            }

            var courses = await courseService.GetAllCoursesWithEnrollmentStatusAsync(studentId.Value);
            return View(mapper.Map<IReadOnlyCollection<CourseViewModel>>(courses));
        }
    }
}