using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
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
    }
}