using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Mvc;
using UniversityAPI.Models;

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private ICourseService courseService;
        private IMapper mapper;
        public CoursesController(ICourseService courseService, IMapper mapper)
        {
            this.courseService = courseService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<CourseDTO>>> GetAllCoursesAsync()
        {
            var courses = await courseService.GetCoursesAsync();

            return Ok(mapper.Map<IReadOnlyCollection<CourseDTO>>(courses));
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
            return Ok(mapper.Map<CourseDTO>(course));
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

            return CreatedAtAction(nameof(GetCourseAsync), new { courseId },
                mapper.Map<CourseDTO>(courseEntity));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourseAsync()
        {

        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartiallyCourseAsync()
        {

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromRoute] int courseId)
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