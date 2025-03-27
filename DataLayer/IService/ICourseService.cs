using Domain.Models.Aggregate;

namespace ApplicationCore.IService
{
    public interface ICourseService
    {
        Task<IReadOnlyCollection<Course>> GetActiveCoursesAsync();
        Task<Course?> GetCourseAsync(int courseId);
    }
}