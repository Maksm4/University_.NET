using ApplicationCore.IService;
using Domain.Models.Aggregate;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly IStudentService StudentService;
        private readonly ICourseService CourseService;
        private readonly UserManager<User> UserManager;
        public CourseController(IStudentService studentService, ICourseService courseService, UserManager<User> userManager)
        {
            StudentService = studentService;
            CourseService = courseService;
            UserManager = userManager;
            
        }

        [Authorize(Roles = "Admin, Student")]
        public async Task<IActionResult> List()
        {
            var courses = await CourseService.GetActiveCoursesAsync();
            return View(courses);
        }

        [HttpGet("courseId")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var currentUser = await UserManager.Users
                .Include(u => u.student)
                .ThenInclude(s => s.LearningPlan)
                .FirstOrDefaultAsync(u => u.Id == UserManager.GetUserId(User));

            if (currentUser == null || currentUser.student == null)
            {
                return NotFound();
            }
            var course = await CourseService.GetCourseAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            
            //something doesnt work here
            currentUser.student.EnrollInCourse(course);
            await StudentService.SaveStudent(currentUser.student);

            return RedirectToAction("List", "Enrollment", new { studentId = currentUser.studentId });
        }
    }
}