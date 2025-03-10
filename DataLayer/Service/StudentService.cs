using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;
using Domain.Models.ValueObject;

namespace ApplicationCore.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
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

        public async Task<IEnumerable<Grade>> GetStudentMarksFromCourse(int studentId, int courseId)
        {
            var student = await studentRepository.GetStudentInfo(studentId);

            if (student == null)
            {
                return Enumerable.Empty<Grade>();
            }

            return student.GetGradesFromCourse(courseId);

        }

        public async Task<IEnumerable<IndividualCourse>> GetStudentCourses(int studentId)
        {
            var student = await studentRepository.GetStudentInfo(studentId);

            if (student == null)
            {
                return Enumerable.Empty<IndividualCourse>();
            }

            return student.GetCourses();
        }

        public async Task<Student> AddLearningPlan(int studentId, LearningPlan learningPlan)
        {
            var student = await studentRepository.GetStudentInfo(studentId);

            if (student == null || learningPlan == null)
            {
                throw new Exception();
            }

            student.LearningPlan = learningPlan;
            await studentRepository.Save();
            return student;
        }
    }
}