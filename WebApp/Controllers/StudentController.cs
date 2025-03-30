using ApplicationCore.IService;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApp.Models.ViewModel;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class StudentController : Controller
    {
        private readonly IStudentService StudentService;
        private readonly UserManager<User> UserManager;
        private readonly IMapper Mapper;

        public StudentController(IStudentService studentService, UserManager<User> userManager, IMapper mapper)
        {
            StudentService = studentService;
            UserManager = userManager;
            Mapper = mapper;
        }

        [Authorize(Roles = Role.Admin)]
        [Route("List")]
        public async Task<IActionResult> AllStudents()
        {
            var students = await StudentService.GetAllStudentsAsync();
            return View(Mapper.Map<IReadOnlyCollection<StudentInfoViewModel>>(students));
        }

        [Authorize(Roles = Role.Student)]
        public async Task<IActionResult> Profile()
        {
            //extract to repiository
            var user = await UserManager.Users
                .Include(u => u.student)
                .FirstOrDefaultAsync(u => u.Id == UserManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }

            return View(Mapper.Map<ProfileViewModel>(user));
        }

        [Authorize(Roles = $"{Role.Student}, {Role.Admin}")]
        [Route("Marks")]
        public async Task<IActionResult> MarksFromCourse(int courseId)
        {
            var userId = UserManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound();
            }

            var student = await StudentService.GetStudentByUserId(userId);

            if (student == null)
            {
                return NotFound();
            }

            var marks = await StudentService.GetStudentMarksForCourseAsync(student.StudentId, courseId);

            return View(Mapper.Map<IReadOnlyCollection<MarkViewModel>>(marks));
        }

        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GiveMark(int mark, int studentId)
        {
            var student = StudentService.GetStudent(studentId);
            if (student == null)
            {
                return NotFound();
            }


        }
    }
}