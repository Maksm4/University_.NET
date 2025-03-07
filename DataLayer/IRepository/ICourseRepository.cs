using ApplicationCore.GenericRepositories;
using Domain.Models;
using Domain.Models.ValueObject;

namespace ApplicationCore.IRepository
{
    public interface ICourseRepository : ICRUDRepository<Course>
    {
        Task<Course?> GetCourseInfo(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesWithModules();
        Task<IEnumerable<CourseCount>> GetCoursesWithModuleCount();
        public Task<IEnumerable<Course>> GetActiveCourses();
        public Task<IEnumerable<Course>> GetCourses(int studentId);
    }
}