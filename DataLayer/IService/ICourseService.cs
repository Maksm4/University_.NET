using ApplicationCore.DTO;
using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IService
{
    public interface ICourseService
    {
        Task<IReadOnlyCollection<Course>> GetCoursesAsync();
        Task<Course?> GetCourseAsync(int courseId);
        Task<IReadOnlyCollection<CourseModule>> GetCourseModules(int courseId);
        Task<IReadOnlyCollection<CourseWithEnrollmentStatusDTO>> GetAllCoursesWithEnrollmentStatusAsync(int studentId);
        Task<IReadOnlyCollection<CourseModuleWithMarkDTO>> GetCourseModulesWithMarkAsync(int studentId, int courseId);
        Task<int?> CreateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int courseId);
    }
}