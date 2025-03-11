using Domain.Models;

namespace ApplicationCore.IRepository
{
    public interface ICourseRepository : ICRUDRepository<Course>
    {
        Task<IEnumerable<Course>> GetActiveCourses();
    }
}