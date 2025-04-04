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

        public async Task<Student> SaveStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException();
            }
            if (studentRepository.IsDetached(student))
            {
                 studentRepository.Update(student);
            }

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

        public async Task<Student?> GetStudentAsync(int studentId)
        {
            return await studentRepository.FindByIdAsync(studentId);
        }

        public async Task<IReadOnlyCollection<MarkedModule>> GetStudentMarksForCourseAsync(int studentId, int courseId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);

            if (student == null)
            {
                return [];
            }

            return student.GetMarksFromCourse(courseId);
        }

        public async Task<IReadOnlyCollection<EnrolledCourse>> GetStudentEnrolledCourses(int studentId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null)
            {
                return [];
            }
            return student.GetEnrolledCourses();
        }

        public async Task<IReadOnlyCollection<EnrolledCourse>> GetEnrolledCoursesAsync(int studentId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null)
            {
                return [];
            }

            return student.GetEnrolledCourses();
        }

        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            var course = await courseRepository.FindByIdAsync(courseId);
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null || course == null)
            {
                return false;
            }

            var coursesTaken = await GetCoursesTakenByStudentAsync(student.StudentId);
            if (!coursesTaken.Contains(course))
            {
                student.EnrollInCourse(course);
                await SaveStudentAsync(student);
                return true;
            }
            return false;
        }

        public async Task<bool> GiveMarkForCourseModuleAsync(int studentId,int courseId, int courseModuleId, int mark)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            var course = await courseRepository.FindByIdAsync(courseId);

            if (student == null || course == null)
            {
                return false;
            }

            var courseModule = course.CourseModules.FirstOrDefault(cm => cm.CourseModuleId == courseModuleId);

            if (courseModule == null)

            {
                return false;
            }

            student.GiveMark(courseModule, mark);
            await SaveStudentAsync(student);
            return true;
        }
    }
}