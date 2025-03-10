using Domain.Models;

namespace ApplicationCore.IService
{
    public interface ICourseService
    {
        public Task<IEnumerable<Course>> GetActiveCourses();
    }
}