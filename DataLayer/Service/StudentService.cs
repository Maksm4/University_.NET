using ApplicationCore.DTO;
using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Domain.Models;
using Domain.Models.Aggregate;
using Domain.Models.VObject;

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

        public async Task<Student?> GetStudentAsync(int studentId)
        {
            return await studentRepository.FindByIdAsync(studentId);
        }

        public async Task<IReadOnlyCollection<StudentCourseTakenDTO?>> GetEnrolledCoursesAsync(int studentId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null)
            {
                return [];
            }

            var enrolledCourses = student.GetEnrolledCourses();
            var courses = await courseRepository.GetCoursesAsync();

            return enrolledCourses.Select(ec =>
            {
                var course = courses.FirstOrDefault(c => c.CourseId == ec.CourseId);
                if (course == null)
                {
                    return null;
                }
                return new StudentCourseTakenDTO
                {
                    CourseId = ec.CourseId,
                    StudentId = studentId,
                    CourseName = course.Name,
                    CourseDescription = course.Description,
                    IsActive = !course.IsDeprecated,
                    DateTimeRange = new DateTimeRange(ec.DateTimeRange.StartTime, ec.DateTimeRange.EndTime)
                };
            }
            ).ToList();
        }

        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            var course = await courseRepository.FindByIdAsync(courseId);
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null || course == null)
            {
                return false;
            }

            var enrolledCourses = student.GetEnrolledCourses().Select(ec => ec.CourseId).ToHashSet();

            if (!enrolledCourses.Contains(courseId))
            {
                student.EnrollInCourse(course); 
                await SaveStudentAsync(student);
                return true;
            }
            return false;
        }

        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId, DateTimeRange DateRange)
        {
            var course = await courseRepository.FindByIdAsync(courseId);
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null || course == null)
            {
                return false;
            }

            var enrolledCourses = student.GetEnrolledCourses().Select(ec => ec.CourseId).ToHashSet();

            if (!enrolledCourses.Contains(courseId))
            {
                student.EnrollInCourse(course, DateRange);
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

        public async Task<int?> CreateStudentAsync(Student student)
        {
            if (student == null)
            {
                return null;
            }
            student = await studentRepository.CreateAsync(student);

            if (student == null)
            {
                return null;
            }

            return student.StudentId;
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null)
            {
                return false;
            }
            studentRepository.Delete(student);
            await SaveStudentAsync(student);
            return true;
        }

        public async Task<bool> StudentExistsAsync(int studentId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null)
            {
                return false;
            }
            return true;
        }

        public async Task WithdrawFromCourse(int studentId, int courseId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null)
            {
                return;
            }

            student.WithdrawFromCourse(courseId);
            await SaveStudentAsync(student);
        }

        public async Task<EnrolledCourse?> GetEnrolledCourseAsync(int studentId, int courseId)
        {
            var student = await studentRepository.FindByIdAsync(studentId);
            if (student == null)
            {
                return null;
            }

            return student.GetEnrolledCourse(courseId);
        }
    }
}