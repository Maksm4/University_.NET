using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationCore.Service
{
    public class StudentReadService : IStudentReadService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;


        public StudentReadService(IStudentRepository studentRepository, ICourseRepository courseRepository)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
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

        public async Task<IEnumerable<Course>> GetCoursesTakenByStudent(int studentId)
        {
            var student = await studentRepository.FindById(studentId);
            var courses = await courseRepository.FindAll();

            if (student == null || courses.IsNullOrEmpty())
            {
                return Enumerable.Empty<Course>();
            }

            var enrolledCourses = student.GetEnrolledCourses();
            if (enrolledCourses.IsNullOrEmpty())
            {
                return Enumerable.Empty<Course>();
            }

            return courses.Where(c => enrolledCourses.Any(ec => ec.CourseId == c.CourseId));
        }

        public async Task<IEnumerable<MarkedModule>> GetStudentMarksForCourse(int studentId, int courseId)
        {
            var student = await studentRepository.FindById(studentId);
            var course = await courseRepository.FindById(courseId);

            if (student == null || course == null)
            {
                return Enumerable.Empty<MarkedModule>();
            }

            return student.GetMarksFromCourse(course);
        }
    }
}