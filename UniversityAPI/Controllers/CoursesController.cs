using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UniversityAPI.Models.Course;

namespace UniversityAPI.Controllers
{
    //[Consumes("application/json")]
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService courseService;
        private readonly IMapper mapper;
        public CoursesController(ICourseService courseService, IMapper mapper)
        {
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoursesAsync()
        {
            var courses = await courseService.GetCoursesAsync();

            return Ok(mapper.Map<IReadOnlyCollection<CourseResponseDTO>>(courses));
        }

        [HttpGet("{courseId}", Name = nameof(GetCourseAsync))]
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

            var courseModules = course.GetCourseModules();


            return Ok(mapper.Map<CourseResponseDTO>(course));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseAsync([FromBody] CourseRequestDTO courseDTO)
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
        public async Task<IActionResult> UpdateCourseAsync([FromRoute] int courseId, [FromBody] CourseRequestDTO courseDTO)
        {
            if (courseId < 0)
            {
                return BadRequest();
            }

            var course = await courseService.GetCourseAsync(courseId);
            if (course == null)
            {
                var courseEntity = mapper.Map<Course>(courseDTO);
                int? createdCourseId = await courseService.CreateCourseAsync(courseEntity);

                if (createdCourseId == null)
                {
                    return BadRequest();
                }

                return CreatedAtRoute(nameof(GetCourseAsync), new { courseId = createdCourseId },
                    mapper.Map<CourseResponseDTO>(courseEntity));
            }

            mapper.Map(courseDTO, course);
            await courseService.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public async Task<IActionResult> PartiallyUpdateCourseAsync([FromRoute] int courseId, [FromBody] JsonPatchDocument<CourseRequestDTO> patchDocument)
        {
            if (courseId < 0 || patchDocument == null)
            {
                return BadRequest();
            }

            var courseEntity = await courseService.GetCourseAsync(courseId);

            if (courseEntity == null)
            {
                return NotFound();
            }

            var courseToPatch = mapper.Map<CourseRequestDTO>(courseEntity);

            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(courseToPatch, courseEntity);
            await courseService.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourseAsync([FromRoute] int courseId)
        {
            if (courseId < 0)
            {
                return BadRequest();
            }

            if(!await courseService.DeleteCourseAsync(courseId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}