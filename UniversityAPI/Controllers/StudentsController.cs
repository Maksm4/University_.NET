using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UniversityAPI.Models.Student;

namespace UniversityAPI.Controllers
{
    [Route("api/students")]
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
        public async Task<IActionResult> CreateStudentAsync([FromBody] StudentRequestDTO studentDTO)
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

        [HttpPut("{studentId}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] int studentId, [FromBody] StudentRequestDTO studentDTO)
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

            mapper.Map(studentDTO, student);

            await studentService.SaveStudentAsync(student);
            return NoContent();
        }

        [HttpPatch("{studentId}")]
        public async Task<IActionResult> PartiallyUpdateCourseAsync([FromRoute] int studentId, [FromBody] JsonPatchDocument<StudentRequestDTO> patchDocument)
        {
            if (studentId < 0 || patchDocument == null)
            {
                return BadRequest();
            }

            var studentEntity = await studentService.GetStudentAsync(studentId);

            if (studentEntity == null)
            {
                return NotFound();
            }

            var studentToPatch = mapper.Map<StudentRequestDTO>(studentEntity);

            patchDocument.ApplyTo(studentToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(studentToPatch, studentEntity);
            await studentService.SaveStudentAsync(studentEntity);
            return NoContent();
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