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
        private readonly IStudentReadService studentService;
        private readonly ICourseReadService courseService;

        public StudentsController(IStudentReadService studentService, ICourseReadService courseService)
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