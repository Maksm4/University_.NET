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

        public async Task<IEnumerable<StudentResponse>> GetAllStudents()
        {
            var students = await StudentRepository.GetAllStudentsWithLearningPlans();

            if (students == null)
            {
                return Enumerable.Empty<StudentResponse>();
            }

            // map 


        }

        public async Task<IEnumerable<MarkResponse>> GetStudentMarksFromCourse(int studentId, int courseId)
        {
            var studentMarks = await StudentRepository.GetStudentmarks(studentId);
            if (studentMarks == null)
            {
                return Enumerable.Empty<MarkResponse>();
            }

            studentMarks = studentMarks.Where(sm => sm.CourseModule.CourseId == courseId);

            //map 
        }
    }
}