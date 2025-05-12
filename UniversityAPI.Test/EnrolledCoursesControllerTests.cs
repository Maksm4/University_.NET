using ApplicationCore.CustomExceptions;
using ApplicationCore.DTO;
using ApplicationCore.IService;
using AutoMapper;
using Domain.Models;
using Domain.Models.VObject;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UniversityAPI.Controllers;
using UniversityAPI.Mappers;
using UniversityAPI.Models.EnrolledCourse;

namespace UniversityAPI.Test
{
    public class EnrolledCoursesControllerTests
    {
        private readonly EnrolledCoursesController enrolledController;
        private Mock<ICourseService> courseServiceMock;
        private Mock<IStudentService> studentServiceMock;
        private EnrolledCourseRequestDTO dummyEnrolledCourseDTO;
        public EnrolledCoursesControllerTests()
        {
            dummyEnrolledCourseDTO = new EnrolledCourseRequestDTO()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5))
            };

            courseServiceMock = new Mock<ICourseService>();
            studentServiceMock = new Mock<IStudentService>();

            var mapperConfiguration = new MapperConfiguration(
                cfg => {
                    cfg.AddProfile<CourseMapper>();
                    cfg.AddProfile<StudentMapper>();
                    }
                );

            var mapper = new Mapper(mapperConfiguration);

            enrolledController = new EnrolledCoursesController(courseServiceMock.Object, studentServiceMock.Object, mapper);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetAllStudentEnrolledCourses_HappyPath_ReturnsOkWithEnrolledCourseDTOs(int studentId)
        {
            //arrange
            studentServiceMock
                .Setup(m => m.GetEnrolledCoursesWithGradesAsync(studentId))
                .ReturnsAsync(new List<StudentCourseTakenDTO>());

            studentServiceMock
                .Setup(m => m.StudentExistsAsync(studentId))
                .ReturnsAsync(true);

            //act
            var result = await enrolledController.GetAllStudentEnrolledCourses(studentId);

            //assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<IList<EnrolledCourseResponseDTO>>(objectResult.Value, exactMatch: false);
        }

        [Fact]
        public async Task GetAllStudentEnrolledCourses_studentIdLessThenZero_ReturnsBadRequest()
        {
            //arrange
            int wrongStudentId = -1;

            //act
            var result = await enrolledController.GetAllStudentEnrolledCourses(wrongStudentId);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetAllStudentEnrolledCourses_NotExistantStudent_ReturnsNotFound()
        {
            //arrange
            int nonExistantStudentId = 5;
            studentServiceMock
                .Setup(m => m.StudentExistsAsync(nonExistantStudentId))
                .ReturnsAsync(false);

            //act
            var result = await enrolledController.GetAllStudentEnrolledCourses(nonExistantStudentId);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public async Task GetStudentEnrolledCourseAsync_HappyPath_ReturnsOkWithEnrolledCourse(int studentId, int courseId)
        {
            //arrange
            studentServiceMock
                .Setup(m => m.GetEnrolledCoursesWithGradesAsync(studentId))
                .ReturnsAsync(new List<StudentCourseTakenDTO>()
                {
                    new StudentCourseTakenDTO()
                    {
                        StudentId = studentId,
                        CourseId = courseId
                    }
                });

            studentServiceMock
                .Setup(m => m.StudentExistsAsync(studentId))
                .ReturnsAsync(true);

            //act
            var result = await enrolledController.GetStudentEnrolledCourseAsync(studentId, courseId);
       
            //assert
            var objectresult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<EnrolledCourseResponseDTO>(objectresult.Value);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-1, -1)]
        public async Task GetStudentEnrolledCourseAsync_NegativeIds_ReturnsBadRequest(int studentId, int courseId)
        {
            //im not sure here if I should mock the methods from services that are used in this method
            // as they shouldnt be hit, but if someone editing th ecode deletes the if statmenet 
            // checking the ids < 0 it would fail the test but with argumentnullexception and im not sure 
            //if its meaningfull enough

            //act
            var result = await enrolledController.GetStudentEnrolledCourseAsync(studentId, courseId);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetStudentEnrolledCourseAsync_StudentNotExistant_ReturnsNotFound()
        {
            //arrange
            int notExistantstudentId = 5;

            studentServiceMock
                .Setup(m => m.StudentExistsAsync(notExistantstudentId))
                .ReturnsAsync(false);

            //act
            var result = await enrolledController.GetStudentEnrolledCourseAsync(notExistantstudentId, It.IsAny<int>());

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetStudentEnrolledCourseAsync_CourseNotExistant_ReturnsNotFound()
        {
            //arrange
            int notExistantCourseId = 5;
            studentServiceMock
               .Setup(m => m.StudentExistsAsync(It.IsAny<int>()))
               .ReturnsAsync(true);

            studentServiceMock
                .Setup(m => m.GetEnrolledCoursesWithGradesAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<StudentCourseTakenDTO>());

            //act
            var result = await enrolledController.GetStudentEnrolledCourseAsync(It.IsAny<int>(), notExistantCourseId);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(0,1)]
        [InlineData(1, 0)]
        public async Task CreateStudentCourseAsync_HappyPath_ReturnsCreatedAtRouteWithCreatedEnrolledCourse(int studentId, int courseId)
        {
            //arrange
            studentServiceMock
               .Setup(m => m.StudentExistsAsync(It.IsAny<int>()))
               .ReturnsAsync(true);

            studentServiceMock
                .Setup(m => m.EnrollStudentInCourseAsync(studentId, courseId, new DateTimeRange(dummyEnrolledCourseDTO.StartDate ?? DateOnly.FromDateTime(DateTime.Now), dummyEnrolledCourseDTO.EndDate)))
                .ReturnsAsync(true);

            //act
            var result = await enrolledController.CreateStudentCourseAsync(studentId, courseId, dummyEnrolledCourseDTO);

            //assert
            var resultObject = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(nameof(EnrolledCoursesController.GetStudentEnrolledCourseAsync), resultObject.RouteName);
            Assert.NotNull(resultObject.RouteValues);
            Assert.Equal(2, resultObject.RouteValues.Count);
            Assert.NotNull(resultObject.RouteValues.GetValueOrDefault("studentId"));
            Assert.NotNull(resultObject.RouteValues.GetValueOrDefault("courseId"));
        }

        [Fact]
        public async Task CreateStudentCourseAsync_EndDateBeforeStartDate_throwsDateRangeException()
        {
            //arrange
            int correctStudentId = 5;
            int correctCourseId = 5;

            studentServiceMock
                .Setup(m => m.StudentExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            studentServiceMock
                .Setup(m => m.EnrollStudentInCourseAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeRange>()))
                .ThrowsAsync(new DateRangeException());

            //act
            var result = await enrolledController.CreateStudentCourseAsync(correctStudentId, correctCourseId, dummyEnrolledCourseDTO);

            //assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(objectResult.Value);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        public async Task CreateStudentCourseAsync_wrongIds_ReturnsBadRequest(int studentId, int courseId)
        {
            //act 
            var result = await enrolledController.CreateStudentCourseAsync(studentId, courseId, dummyEnrolledCourseDTO);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        public async Task DeleteStudentCourseAsync_wrongIds_ReturnsBadRequest(int studentId, int courseId)
        {
            //act 
            var result = await enrolledController.DeleteStudentCourseAsync(studentId, courseId);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteStudentCourseAsync_enrolledCourseNotExistant_ReturnsNotFound()
        {
            //arrange
            studentServiceMock
                .Setup(m => m.GetEnrolledCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((EnrolledCourse?)null);

            studentServiceMock
               .Setup(m => m.StudentExistsAsync(It.IsAny<int>()))
               .ReturnsAsync(true);

            courseServiceMock
               .Setup(m => m.CourseExists(It.IsAny<int>()))
               .ReturnsAsync(true);

            //act 
            var result = await enrolledController.DeleteStudentCourseAsync(5, 5);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateStudentCourseAsync_HappyPathFullUpdate_ReturnsNoContent()
        {
            //arrange
            studentServiceMock
                .Setup(m => m.GetEnrolledCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new EnrolledCourse(1,1, new DateTimeRange(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now))));

            //act
            var result = await enrolledController.UpdateStudentCourseAsync(5,5, dummyEnrolledCourseDTO);

            //assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateStudentCourseAsync_HappyPathPartialUpdate_ReturnsNoContent()
        {
            //arrange
            var enrolledCourseDTO = new EnrolledCourseRequestDTO()
            {
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
            };

            studentServiceMock
                .Setup(m => m.GetEnrolledCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new EnrolledCourse(1, 1, new DateTimeRange(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now))));

            //act
            var result = await enrolledController.UpdateStudentCourseAsync(5, 5, enrolledCourseDTO);

            //assert
            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        public async Task UpdateStudentCourseAsync_WrongIds_ReturnsBadRequest(int studentId, int courseId)
        {
            //act
            var result = await enrolledController.UpdateStudentCourseAsync(studentId, courseId, dummyEnrolledCourseDTO);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateStudentCourseAsync_NotExistantEnrolledCourse_ReturnsNotFound()
        {
            //arrange
            studentServiceMock
                .Setup(m => m.GetEnrolledCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((EnrolledCourse?)null);

            //act
            var result = await enrolledController.UpdateStudentCourseAsync(5,5, dummyEnrolledCourseDTO);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateStudentCourseAsync_EndTimeBeforeStartTimeThrowsException_ReturnsBadRequest()
        {
            //arrange
            studentServiceMock
              .Setup(m => m.GetEnrolledCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
              .ThrowsAsync(new AutoMapperMappingException("", new DateRangeException()));

            //act
            var result = await enrolledController.UpdateStudentCourseAsync(5, 5, dummyEnrolledCourseDTO);

            //assert
            var objectresult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<string>(objectresult.Value);
        }
    }
}