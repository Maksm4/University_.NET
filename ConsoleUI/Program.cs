using ApplicationCore.IService;
using AutoMapper;
using ConsoleUI.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleUI
{
    public class Program
    {
        private static IStudentService studentService;
        private static ICourseService courseService;
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            var app = Configuration.IntitializeServices(args);
            studentService = app.Services.GetRequiredService<IStudentService>();
            courseService = app.Services.GetRequiredService<ICourseService>();
            mapper = app.Services.GetRequiredService<IMapper>();

            while (true)
            {
                Console.WriteLine("1.Get all active courses");
                Console.WriteLine("2.Get all students");
                Console.WriteLine("3.Get student courses");
                Console.WriteLine("4.Get student marks from course");

                if (int.TryParse(Console.ReadLine(), out int decision))
                {
                    switch (decision)
                    {
                        case 1:
                            {
                                var activeCourses = GetActiveCourses();
                                foreach (var course in activeCourses)
                                {
                                    Console.WriteLine($"name: {course.Name} description: {course.Description}");
                                }
                            }
                            break;
                        case 2:
                            {
                                var students = GetStudents();
                                foreach (var student in students)
                                {
                                    Console.WriteLine($"{student.FirstName} {student.LastName} learning plan name: {student.LearningPlan.Name}");
                                }
                            }
                            break;
                        case 3:
                            {
                                Console.WriteLine("enter student id");
                                if (int.TryParse(Console.ReadLine(), out int studentId))
                                {
                                    var courses = GetStudentCourses(studentId);
                                    foreach (var course in courses)
                                    {
                                        Console.WriteLine($"{course.Name} {course.Description}");
                                    }
                                }
                            }
                            break;
                        case 4:
                            {
                                if (int.TryParse(Console.ReadLine(), out int studentId) &&
                                    int.TryParse(Console.ReadLine(), out int courseId))
                                {
                                    var studentMarks = GetMarksFromCourse(studentId, courseId);
                                    Console.WriteLine($"Marks for:\n studentId: {studentId} courseId: {courseId}");
                                    foreach (var mark in studentMarks)
                                    {
                                        Console.WriteLine($"courseModule:{mark.CourseModuleId} {mark.CourseModule.Description} mark: {mark.Mark}");
                                    }
                                }
                            }
                            break;
                    }
                }
                Console.WriteLine("something went wrong, choose again");
            }
        }

        private static IEnumerable<CourseResponse> GetActiveCourses()
        {
            var activeCourses = courseService.GetActiveCourses();

            return mapper.Map<IEnumerable<CourseResponse>>(activeCourses);
        }

        private static IEnumerable<StudentResponse> GetStudents()
        {
            var students = studentService.GetAllStudents();

            return mapper.Map<IEnumerable<StudentResponse>>(students);
        }
        private static IEnumerable<CourseResponse> GetStudentCourses(int studentId)
        {
            var studentCourses = studentService.GetStudentCourses(studentId);

            return mapper.Map<IEnumerable<CourseResponse>>(studentCourses);
        }

        private static StudentDTO GetStudent(int studentId)
        {
          
        }

        private static IEnumerable<ModuleMarkDTo> GetMarksFromCourse(int studentId, int courseId)
        {
                
        }
    }
}
