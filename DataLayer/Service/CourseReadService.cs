using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;

namespace ApplicationCore.Service
{
    public class CourseReadService : ICourseReadService
    {
        private readonly ICourseRepository courseRepository;

        public CourseReadService(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }
        public async Task<IEnumerable<Course>> GetActiveCourses()
        {
            var courses = await courseRepository.GetActiveCourses();

            if (courses == null || courses.Count() == 0)
            {
                return Enumerable.Empty<Course>();
            }

            return courses;
        }
    }
}