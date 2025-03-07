using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;

namespace Infrastructure.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task<Student?> GetStudent(int studentId)
        {
            var student = await studentRepository.GetStudentInfo(studentId);

            // for testing
            if (student == null)
            {
                throw new Exception();
            }
            return student;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            var students = await studentRepository.GetAllStudentsWithLearningPlans();

            if (students == null)
            {
                return Enumerable.Empty<Student>();
            }

            return students;
        }

        public async Task<IEnumerable<ModuleMark>> GetStudentMarksFromCourse(int studentId, int courseId)
        {
            var studentMarks = await studentRepository.GetStudentmarks(studentId);
            if (studentMarks == null)
            {
                return Enumerable.Empty<ModuleMark>();
            }

            studentMarks = studentMarks.Where(sm => sm.CourseModule.CourseId == courseId);

            return studentMarks;
        }

    }
}