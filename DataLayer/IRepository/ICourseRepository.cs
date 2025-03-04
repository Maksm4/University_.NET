using DataLayer.GenericRepositories;
using DataLayer.Models;
using DataLayer.Models.ValueObject;

namespace DataLayer.IRepository
{
    public interface ICourseRepository : ICRUDRepository<Course>
    {
        Task<Course?> GetCourseInfo(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesWithModules();
        Task<IEnumerable<CourseCount>> GetCoursesWithModuleCount();
        Task<IEnumerable<IndividualCourse>> GetStartedCourses();
    }
}