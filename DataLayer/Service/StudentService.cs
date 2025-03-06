using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Models;
using ApplicationCore.Models.DTOs;

namespace Infrastructure.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository StudentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            StudentRepository = studentRepository;
        }

        public Task<IEnumerable<StudentResponse>> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ModuleMark>> GetStudentMarksFromCourse(int studentId, int courseId)
        {
            throw new NotImplementedException();
        }
    }
}