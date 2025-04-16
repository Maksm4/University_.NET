using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UniversityAPI.Models;

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
        public async Task<ActionResult<IReadOnlyCollection<CourseResponseDTO>>> GetAllCoursesAsync()
        {
            var courses = await courseService.GetCoursesAsync();

            return Ok(mapper.Map<IReadOnlyCollection<CourseResponseDTO>>(courses));
        }

        [HttpGet("{courseId}", Name = "GetCourseAsync")]
        public async Task<IActionResult> GetCourseAsync([FromRoute] int courseId)
        {
            if (courseId < 0)
            {
                return BadRequest();
            }
            var course = await courseService.GetCourseAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CourseResponseDTO>(course));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseAsync([FromBody] CourseForCreationDTO courseDTO)
        {
            if (courseDTO == null)
            {
                return BadRequest();
            }

            var courseEntity = mapper.Map<Course>(courseDTO);
            int? courseId = await courseService.CreateCourseAsync(courseEntity);
            if (courseId == null)
            {
                return BadRequest();
            }

            return CreatedAtRoute(nameof(GetCourseAsync), new { courseId },
                mapper.Map<CourseResponseDTO>(courseEntity));
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourseAsync([FromRoute] int courseId, [FromBody] CourseForUpdateDTO courseDTO)
        {
            if (courseId < 0)
            {
                return BadRequest();
            }

            var course = await courseService.GetCourseAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            mapper.Map(courseDTO, course);
            await courseService.SaveCourseAsync(course);
            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public async Task<IActionResult> PartiallyUpdateCourseAsync([FromRoute] int courseId, [FromBody] JsonPatchDocument<CourseForUpdateDTO> patchDocument)
        {
            if (courseId < 0)
            {
                return BadRequest();
            }

            var courseEntity = await courseService.GetCourseAsync(courseId);

            if (courseEntity == null)
            {
                return NotFound();
            }

            var courseToPatch = mapper.Map<CourseForUpdateDTO>(courseEntity);

            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(courseToPatch, courseEntity);
            await courseService.SaveCourseAsync(courseEntity);
            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int courseId)
        {
            if (courseId < 0)
            {
                return BadRequest();
            }

            if (!await courseService.DeleteCourseAsync(courseId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}