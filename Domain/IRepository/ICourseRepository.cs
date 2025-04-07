using Domain.Models.Aggregate;

namespace ApplicationCore.IRepository
{
    public interface ICourseRepository : ICRUDRepository<Course>
    {
        Task<IReadOnlyCollection<Course>> GetCoursesAsync();
    }
}