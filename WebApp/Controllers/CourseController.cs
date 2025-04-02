using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper Mapper;
        public CourseController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager, IMapper mapper) 
        {
            StudentService = studentService;
            CourseService = courseService;
            Mapper = mapper;
        }

        [Route("ListAdmin")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> AllCoursesAdminAsync()
        {
            var courses = await CourseService.GetActiveCoursesAsync();

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
            var courses = await CourseService.GetActiveCoursesAsync();

            if (CurrentUser == null)
            {
                return NotFound();
            }


            var student = await StudentService.GetStudentByUserIdAsync(CurrentUser.Id);
            if (student == null)
            {
                return NotFound();
            }

            var studentCourses = student.GetEnrolledCourses();

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