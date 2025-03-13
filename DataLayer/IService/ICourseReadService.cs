using Domain.Models;

namespace ApplicationCore.IService
{
    public interface ICourseReadService
    {
        Task<IEnumerable<Course>> GetActiveCourses();
    }
}