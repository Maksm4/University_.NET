using ApplicationCore.IRepository;
using ApplicationCore.IService;
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
        public async Task<IReadOnlyCollection<Course>> GetActiveCoursesAsync()
        {
            return await courseRepository.GetActiveCoursesAsync();
        }
    }
}