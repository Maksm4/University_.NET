using Domain.Models;

namespace ApplicationCore.IService
{
    public interface IStudentService
    {
        public Task<IEnumerable<Student>> GetAllStudents();
        public Task<IEnumerable<ModuleMark>> GetStudentMarksFromCourse(int studentId, int courseId);
        Task<Student?> GetStudent(int studentId);
        //public Task<IEnumerable<Course>> GetStudentCourses(int studentId);
    }
}