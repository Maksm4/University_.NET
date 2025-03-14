using Domain.Models.Aggregate;

namespace ApplicationCore.IService
{
    public interface ICourseReadService
    {
        Task<IEnumerable<Course>> GetActiveCourses();
    }
}