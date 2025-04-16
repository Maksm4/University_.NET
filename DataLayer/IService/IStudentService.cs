using ApplicationCore.DTO;
using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IService
{
    public interface IStudentService
    {
        Task<IReadOnlyCollection<Student>> GetAllStudentsAsync();
        //Task<IReadOnlyCollection<Course>> GetCoursesTakenByStudentAsync(int studentId);
        //Task<IReadOnlyCollection<MarkedModule>> GetStudentMarksForCourseAsync(int studentId, int courseId);
        Task<Student?> GetStudentAsync(int studentId);
        Task<Student> SaveStudentAsync(Student student);
        Task<IReadOnlyCollection<StudentCourseTakenDTO?>> GetEnrolledCoursesAsync(int studentId);
        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId);
        Task<bool> GiveMarkForCourseModuleAsync(int studentId, int courseId, int courseModuleId, int mark);
        Task<int?> CreateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int studentId);
    }
}