using ApplicationCore.DTO;
using Domain.Models;
using Domain.Models.Aggregate;
using Domain.Models.VObject;

namespace ApplicationCore.IService
{
    public interface IStudentService
    {
        Task<IReadOnlyCollection<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentAsync(int studentId);
        Task<Student> SaveStudentAsync(Student student);
        Task<IReadOnlyCollection<StudentCourseTakenDTO?>> GetEnrolledCoursesWithGradesAsync(int studentId);
        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId);
        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId, DateTimeRange DateRange);
        Task<bool> GiveMarkForCourseModuleAsync(int studentId, int courseId, int courseModuleId, int mark);
        Task<int?> CreateStudentAsync(Student student);
        Task WithdrawFromCourse(int studentId, int courseId);
        Task<bool> DeleteStudentAsync(int studentId);
        Task<bool> StudentExistsAsync(int studentId);
        Task<EnrolledCourse?> GetEnrolledCourseAsync(int studentId, int courseId);
    }
}