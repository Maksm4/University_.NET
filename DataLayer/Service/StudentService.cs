using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;


        public StudentService(IStudentRepository studentRepository, ICourseRepository courseRepository)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
        }

        public async Task<Student> SaveStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException();
            }
            if (studentRepository.FindByIdAsync(student.StudentId) == null)
            {
                return await studentRepository.CreateAsync(student);
            }
            studentRepository.Update(student);
            await studentRepository.SaveAsync();
            return student;
        }

        public async Task<IReadOnlyCollection<Student>> GetAllStudentsAsync()
        {
            return await studentRepository.GetAllStudentsWithEnrolledCoursesAsync();
        }

        public async Task<IReadOnlyCollection<Course>> GetCoursesTakenByStudentAsync(int studentId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);

            if (student == null)
            {
                return [];
            }

            var enrolledCourses = student.GetEnrolledCourses();

            //ienumerable 
            var courseIds = enrolledCourses.Select(ec => ec.CourseId);
            return await courseRepository.GetActiveCoursesAsync(courseIds);
        }

        public async Task<Student?> GetStudent(int studentId)
        {
            return await studentRepository.FindByIdAsync(studentId);
        }

        public async Task<IReadOnlyCollection<MarkedModule>> GetStudentMarksForCourseAsync(int studentId, int courseId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            var course = await courseRepository.FindByIdAsync(courseId);

            if (student == null || course == null)
            {
                return [];
            }

            return student.GetMarksFromCourse(course);
        }
    }
}