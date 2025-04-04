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
                                var activeCourses = GetActiveCourses().Result;
                                foreach (var course in activeCourses)
                                {
                                    Console.WriteLine($"name: {course.Name} description: {course.Description}");
                                }
                            }
                            break;
                        case 2:
                            {
                                var students = GetStudents().Result;
                                foreach (var student in students)
                                {
                                    Console.WriteLine($"first name: {student.FirstName} last name: {student.LastName} birth date: {student.BirthDate}");
                                }
                            }
                            break;
                        case 3:
                            {
                                Console.WriteLine("enter student id");
                                if (int.TryParse(Console.ReadLine(), out int studentId))
                                {
                                    var courses = GetStudentCourses(studentId).Result;
                                    foreach (var course in courses)
                                    {
                                        Console.WriteLine($"{course.Name} {course.Description}");
                                    }
                                }
                            }
                            break;
                        case 4:
                            {
                                Console.WriteLine("enter student id and click enter. then courseId and enter again");
                                if (int.TryParse(Console.ReadLine(), out int studentId) &&
                                    int.TryParse(Console.ReadLine(), out int courseId))
                                {
                                    var studentMarks = GetMarksFromCourse(studentId, courseId).Result;
                                    Console.WriteLine($"Marks for:\n studentId: {studentId} courseId: {courseId}");
                                    foreach (var mark in studentMarks)
                                    {
                                        Console.WriteLine($"courseModule: {mark.CourseModuleId} mark: {mark.Mark}");
                                    }
                                }
                            }
                            break;
                        default :
                            {
                                Console.WriteLine("you choose not viable option, choose again");
                            }
                            break;
                    }
                }
            }
        }

        private async static Task<IEnumerable<CourseResponse>> GetActiveCourses()
        {
            var activeCourses = await courseService.GetCoursesAsync();

            return mapper.Map<IEnumerable<CourseResponse>>(activeCourses);
        }

        private async static Task<IEnumerable<StudentResponse>> GetStudents()
        {
            var students = await studentService.GetAllStudentsAsync();

            return mapper.Map<IEnumerable<StudentResponse>>(students);
        }

        private async static Task<IEnumerable<CourseResponse>> GetStudentCourses(int studentId)
        {
            var studentCourses = await studentService.GetCoursesTakenByStudentAsync(studentId);

            return mapper.Map<IEnumerable<CourseResponse>>(studentCourses);
        }

        private async static Task<IEnumerable<MarkResponse>> GetMarksFromCourse(int studentId, int courseId)
        {
            var markedModules =await studentService.GetStudentMarksForCourseAsync(studentId, courseId);

            return mapper.Map<IEnumerable<MarkResponse>>(markedModules);
        }
    }
}
