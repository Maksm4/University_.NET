using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Models;
using ApplicationCore.Models.DTOs;

namespace Infrastructure.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }
        public Task<IEnumerable<Course>> GetActiveCourses()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CourseResponse>> GetStudentCourses(int studentId)
        {
            throw new NotImplementedException();
        }
    }
}