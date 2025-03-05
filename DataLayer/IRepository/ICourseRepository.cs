using ApplicationCore.GenericRepositories;
using ApplicationCore.Models;
using ApplicationCore.Models.ValueObject;

namespace ApplicationCore.IRepository
{
    public interface ICourseRepository : ICRUDRepository<Course>
    {
        Task<Course?> GetCourseInfo(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesWithModules();
        Task<IEnumerable<CourseCount>> GetCoursesWithModuleCount();
        public Task<IEnumerable<IndividualCourse>> GetActiveCourses();
        public Task<IEnumerable<Course>> GetCourses(int studentId);
    }
}