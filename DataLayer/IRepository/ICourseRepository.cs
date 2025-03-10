using ApplicationCore.GenericRepositories;
using Domain.Models;

namespace ApplicationCore.IRepository
{
    public interface ICourseRepository : ICRUDRepository<Course>
    {
        Task<Course?> GetCourseInfo(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesWithModules();
        public Task<IEnumerable<Course>> GetActiveCourses();
    }
}