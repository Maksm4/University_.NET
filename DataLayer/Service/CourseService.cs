using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Models;
using ApplicationCore.Models.DTOs;
using AutoMapper;

namespace Infrastructure.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }
        public async Task<IEnumerable<CourseResponse>> GetActiveCourses()
        {
            var courses = await courseRepository.GetActiveCourses();

            if (courses == null || courses.Count() == 0)
            {
                return Enumerable.Empty<CourseResponse>();
            }

            //inject this config
            var config = new MapperConfiguration(conf => conf.CreateMap<Course, CourseResponse>());

            var mapper = new Mapper(config);

            var coursesResponse = mapper.Map<IEnumerable<Course>, IEnumerable<CourseResponse>>(courses);
            if (coursesResponse == null)
            {
                throw new Exception(); //change to custom exception
            }
            return coursesResponse;
        }

        public async Task<IEnumerable<CourseResponse>> GetStudentCourses(int studentId)
        {
            if (studentId < 0)
            {
                return Enumerable.Empty<CourseResponse>();
            }

            var courses = await courseRepository.GetAllCoursesWithModules();
            if (courses == null || courses.Count() == 0)
            {
                return Enumerable.Empty<CourseResponse>();
            }

            //map 
        }
    }
}