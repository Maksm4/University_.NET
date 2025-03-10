using Domain.Models;

namespace ApplicationCore.IService
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetActiveCourses();
    }
}