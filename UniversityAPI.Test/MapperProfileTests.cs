using ApplicationCore.DTO;
using AutoMapper;
using Domain.Models;
using Domain.Models.Aggregate;
using Domain.Models.VObject;
using UniversityAPI.Mappers;
using UniversityAPI.Models.EnrolledCourse;
using UniversityAPI.Models.Student;

namespace UniversityAPI.Test
{
    public class MapperProfileTests
    {
        private readonly IMapper mapper;

        public MapperProfileTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentMapper>();
                cfg.AddProfile<CourseMapper>();
            });

            mapper = mapperConfig.CreateMapper();   
        }

        //[Fact]
        //public void AutoMapper_configuration_ReturnsIsValid()
        //{
        //    mapper.ConfigurationProvider.AssertConfigurationIsValid();
        //}

        [Fact]
        public void StudentProfile_Student_To_StudentResponseDTO()
        {
            //arrange
            var student = new Student("testname", "testlastname", DateOnly.FromDateTime(DateTime.Now), new LearningPlan("testname"));

            //act
            var studentResponse = mapper.Map<StudentResponseDTO>(student);

            //assert
            Assert.NotNull(studentResponse);
            Assert.Equal(studentResponse.FirstName, student.FirstName);
            Assert.Equal(studentResponse.LastName, student.LastName);
            Assert.Equal(studentResponse.BirthDate, student.BirthDate);
        }

        [Fact]
        public void StudentProfile_StudentRequestDTO_To_Student()
        {
            //arrange
            var studentRequest = new StudentRequestDTO()
            { 
                FirstName = "Test",
                LastName = "Test",
                BirthDate = DateOnly.FromDateTime(DateTime.Now)
            };

            //act
            var student = mapper.Map<Student>(studentRequest);

            //assert
            Assert.NotNull(student);
            Assert.Equal(student.FirstName, studentRequest.FirstName);
            Assert.Equal(student.LastName, studentRequest.LastName);
            Assert.Equal(student.BirthDate, studentRequest.BirthDate);
        }


        [Fact]
        public void StudentProfile_Student_To_StudentRequestDTO()
        {
            //arrange
            var student = new Student("testname", "testlastname", DateOnly.FromDateTime(DateTime.Now), new LearningPlan("testname"));

            //act
            var studentRequest = mapper.Map<StudentRequestDTO>(student);

            //assert
            Assert.NotNull(studentRequest);
            Assert.Equal(studentRequest.FirstName, student.FirstName);
            Assert.Equal(studentRequest.LastName, student.LastName);
            Assert.Equal(studentRequest.BirthDate, student.BirthDate);
        }

        [Fact]
        public void StudentProfile_StudentCourseTakenDTO_To_EnrolledCourseResponseDTO()
        {
            //arrange
            var courseTakenDTO = new StudentCourseTakenDTO()
            {
                StudentId = 1,
                CourseId = 1,
                CourseName = "Test",
                CourseDescription = "Test",
                IsActive = true,
                DateTimeRange = new DateTimeRange(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now))
            };

            //act
            var enrolledCourseResponse = mapper.Map<EnrolledCourseResponseDTO>(courseTakenDTO);

            //assert
            Assert.NotNull(enrolledCourseResponse);
            Assert.Equal(enrolledCourseResponse.CourseId, courseTakenDTO.StudentId);
            Assert.Equal(enrolledCourseResponse.CourseName, courseTakenDTO.CourseName);
            Assert.Equal(enrolledCourseResponse.CourseDescription, courseTakenDTO.CourseDescription);
            Assert.Equal(enrolledCourseResponse.IsActive, courseTakenDTO.IsActive);
            Assert.Equal(enrolledCourseResponse.StartDate, courseTakenDTO.DateTimeRange.StartTime);
            Assert.Equal(enrolledCourseResponse.EndDate, courseTakenDTO.DateTimeRange.EndTime);
        }

        [Fact]
        public void StudentProfile_EnrolledCourseRequestDTO_To_EnrolledCourse()
        {
            //arrange
            var enrolledCourseRequestDTO = new EnrolledCourseRequestDTO()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now)
            };

            //act
            var enrolledCourse = mapper.Map<EnrolledCourse>(enrolledCourseRequestDTO);

            //assert
            Assert.NotNull(enrolledCourse);
            Assert.Equal(enrolledCourse.DateTimeRange.StartTime, enrolledCourseRequestDTO.StartDate);
            Assert.Equal(enrolledCourse.DateTimeRange.EndTime, enrolledCourseRequestDTO.EndDate);
        }
    }
}