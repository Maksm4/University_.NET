using ApplicationCore.IRepository;
using ApplicationCore.IService;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository courseRepository)
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