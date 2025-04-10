using ApplicationCore.DTO;
using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;
        private readonly IStudentRepository studentRepository;

        public CourseService(ICourseRepository courseRepository, IStudentRepository studentRepository)
        {
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
        }
        public async Task<IReadOnlyCollection<Course>> GetCoursesAsync()
        {
            return await courseRepository.GetCoursesAsync();
        }

        public async Task<Course?> GetCourseAsync(int courseId)
        {
            return await courseRepository.FindByIdAsync(courseId);
        }

        public async Task<IReadOnlyCollection<CourseModule>> GetCourseModules(int courseId)
        {
            var course = await courseRepository.FindByIdAsync(courseId);

            if (course == null)
            {
                return [];
            }

            return course.CourseModules;
        }

        public async Task<IReadOnlyCollection<CourseWithEnrollmentStatusDTO>> GetAllCoursesWithEnrollmentStatusAsync(int studentId)
        {
            var courses = await courseRepository.GetCoursesAsync();
            var student = await studentRepository.FindByIdAsync(studentId);

            if (student == null)
            {
                return [];
            }

            var enrolledCourseIds = student.GetEnrolledCourses().Select(ec => ec.CourseId).ToHashSet();

            return courses.Select(c =>
            {
                return new CourseWithEnrollmentStatusDTO
                {
                    CourseId = c.CourseId,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = !c.IsDeprecated,
                    IsEnrolled = enrolledCourseIds.Contains(c.CourseId),
                };
            }).ToList();
        }

        public async Task<IReadOnlyCollection<CourseModuleWithMarkDTO>> GetCourseModulesWithMarkAsync(int studentId, int courseId)
        {
            var course = await courseRepository.FindByIdAsync(courseId);
            var student = await studentRepository.FindByIdAsync(studentId);
            if (course == null || student == null)
            {
                return [];
            }

            var courseModules = course.GetCourseModules();
            var studentMarks = student.GetMarksFromCourse(courseId);

            return courseModules.Select(cm =>
            {
                return new CourseModuleWithMarkDTO
                {
                    Mark = studentMarks.FirstOrDefault(sm => sm.CourseModuleId == cm.CourseModuleId)?.Mark,
                    Description = cm.Description,
                    CourseModuleId = cm.CourseModuleId
                };
            }).ToList();
        }
    }
}