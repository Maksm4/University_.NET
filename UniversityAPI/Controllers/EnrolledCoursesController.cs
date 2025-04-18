using ApplicationCore.CustomExceptions;
using ApplicationCore.IService;
using AutoMapper;
using Domain.Models;
using Domain.Models.VObject;
using Microsoft.AspNetCore.Mvc;
using UniversityAPI.Models.EnrolledCourse;

namespace UniversityAPI.Controllers
{
    [Route("api/students/{studentId}/courses")]
    [ApiController]
    public class EnrolledCoursesController : ControllerBase
    {
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;
        private IMapper mapper;
        public EnrolledCoursesController(ICourseService courseService,IStudentService studentService, IMapper mapper)
        {
            this.courseService = courseService;
            this.mapper = mapper;
            this.studentService = studentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllStudentEnrolledCourses([FromRoute] int studentId)
        {
            if (studentId < 0)
            {
                return BadRequest();
            }

            if (! await studentService.StudentExistsAsync(studentId))
            {
                return NotFound();
            }

            var studentCourses = await studentService.GetEnrolledCoursesWithGradesAsync(studentId);
            return Ok(mapper.Map<IReadOnlyCollection<EnrolledCourseResponseDTO>>(studentCourses));
        }

        [HttpGet("{courseId}", Name = nameof(GetStudentEnrolledCourseAsync))]
        public async Task<IActionResult> GetStudentEnrolledCourseAsync([FromRoute] int studentId, [FromRoute] int courseId)
        {
            if (studentId < 0 || courseId < 0)
            {
                return BadRequest();
            }

            if (!await studentService.StudentExistsAsync(studentId))
            {
                return NotFound();
            }

            var studentCourses = await studentService.GetEnrolledCoursesWithGradesAsync(studentId);
            var course = studentCourses.FirstOrDefault(enrolledCrs => enrolledCrs?.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<EnrolledCourseResponseDTO>(course));
        }

        [HttpPost("{courseId}")]
        public async Task<IActionResult> CreateStudentCourseAsync([FromRoute] int studentId, [FromRoute] int courseId, [FromBody] EnrolledCourseRequestDTO enrolledCourseDTO)
        {
            try
            {
                if (studentId < 0 || enrolledCourseDTO == null ||
                    enrolledCourseDTO.StartDate?.CompareTo(enrolledCourseDTO.EndDate) > 0)
                {
                    return BadRequest();
                }

                if (!await studentService.StudentExistsAsync(studentId))
                {
                    return NotFound();
                }

                if (!await studentService.EnrollStudentInCourseAsync(studentId, courseId,
                    new DateTimeRange(enrolledCourseDTO.StartDate ?? DateOnly.FromDateTime(DateTime.Now), enrolledCourseDTO.EndDate)))
                {
                    return BadRequest();
                }

                return CreatedAtRoute(nameof(GetStudentEnrolledCourseAsync),
                    new { studentId, courseId }, new { });

            }catch (DateRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteStudentCourseAsync([FromRoute] int studentId, [FromRoute] int courseId)
        {
            if (studentId < 0 || courseId < 0)
            {
                return BadRequest();
            }

            if (!await studentService.StudentExistsAsync(studentId) || !await courseService.CourseExists(courseId))
            {
                return NotFound();
            }

            if (await studentService.GetEnrolledCourseAsync(studentId, courseId) == null)
            {
                return NotFound();
            }

            await studentService.WithdrawFromCourse(studentId, courseId);
            return NoContent();
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateStudentCourseAsync([FromRoute] int studentId, [FromRoute] int courseId, [FromBody] EnrolledCourseRequestDTO enrolledCourseDTO)
        {
            try
            {
                if (studentId < 0 || enrolledCourseDTO == null)
                {
                    return BadRequest();
                }

                EnrolledCourse? enrolledCourseEntity = await studentService.GetEnrolledCourseAsync(studentId, courseId);

                if (enrolledCourseEntity == null)
                {
                    return NotFound();
                }

                mapper.Map(enrolledCourseDTO, enrolledCourseEntity);
                await courseService.SaveAsync();
                return NoContent();

            }catch (DateRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}