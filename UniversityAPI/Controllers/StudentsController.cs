using ApplicationCore.Context;
using ApplicationCore.IService;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;

        public StudentsController(IStudentService studentService, ICourseService courseService)
        {
            this.studentService = studentService;
            this.courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = studentService.GetAllStudents();
            return Ok(students);
        }
    }
}