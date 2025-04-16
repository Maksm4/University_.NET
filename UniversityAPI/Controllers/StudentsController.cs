using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Mvc;
using UniversityAPI.Models.Student;

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly IMapper mapper;

        public StudentsController(IStudentService studentService, IMapper mapper)
        {
            this.studentService = studentService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await studentService.GetAllStudentsAsync();

            return Ok(mapper.Map<IReadOnlyCollection<StudentResponseDTO>>(students));
        }

        [HttpGet("{studentId}", Name = nameof(GetStudentAsync))]
        public async Task<IActionResult> GetStudentAsync([FromRoute] int studentId)
        {
            if (studentId < 0)
            {
                return BadRequest();
            }
            var student = await studentService.GetStudentAsync(studentId);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<StudentResponseDTO>(student));
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudentAsync([FromBody] StudentForCreationDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return BadRequest();
            }

            var studentEntity = mapper.Map<Student>(studentDTO);

            int? studentId = await studentService.CreateStudentAsync(studentEntity);
            if (studentId == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute(nameof(GetStudentAsync), new { studentId },
                mapper.Map<StudentResponseDTO>(studentEntity));
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudentAsync([FromRoute] int studentId)
        {
            if (studentId < 0)
            {
                return BadRequest();
            }

            if (!await studentService.DeleteStudentAsync(studentId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}