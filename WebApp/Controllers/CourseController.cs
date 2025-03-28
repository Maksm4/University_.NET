using ApplicationCore.IService;
using AutoMapper;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class CourseController : Controller
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly UserManager<User> UserManager;
        private readonly IMapper Mapper;
        public CourseController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager, IMapper mapper)
        {
            StudentService = studentService;
            CourseService = courseService;
            UserManager = userManager;
            Mapper = mapper; 
        }

        [Route("List")]
        [Authorize(Roles = $"{Role.Admin}, {Role.Student}")]
        public async Task<IActionResult> AllCourses()
        {
            var currUser = await UserManager.GetUserAsync(User);
            var courses = await CourseService.GetActiveCoursesAsync();

            if (currUser == null)
            {
                return NotFound();
            }

            if (await UserManager.IsInRoleAsync(currUser, Role.Admin))
            {
                return View(courses.Select(
                    c => 
                    new CourseViewModel
                    {
                       CourseId = c.CourseId,
                       Name = c.Name,
                       Description = c.Description,
                       IsActive = !c.IsDeprecated,
                       isEnrolled = null
                    }));
            }

            var student = await StudentService.GetStudentByUserId(currUser.Id);
            if (student == null)
            {
                return NotFound();
            }

            var studentCourses = student.GetEnrolledCourses();

            return View(courses.Select(c =>
            new CourseViewModel
            {
                CourseId =c.CourseId,
                Name = c.Name,
                Description =c.Description,
                IsActive = !c.IsDeprecated,
                isEnrolled = studentCourses.Any(sc => sc.CourseId == c.CourseId)
            }));
        }
    }
}