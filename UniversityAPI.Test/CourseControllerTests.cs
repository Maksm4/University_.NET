using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UniversityAPI.Controllers;
using UniversityAPI.Mappers;
using UniversityAPI.Models.Course;

namespace UniversityAPI.Test
{
    public class CourseControllerTests
    {
        private readonly CoursesController courseController;
        private Mock<ICourseService> courseServiceMock;
        private readonly Course dummyCourse;
        public CourseControllerTests()
        {
            dummyCourse = new Course(0, "testName0", "testDescription0", false);
            courseServiceMock = new Mock<ICourseService>();
            
            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<CourseMapper>()
                );
            var mapper = new Mapper(mapperConfiguration);

            courseController = new CoursesController(courseServiceMock.Object, mapper);
        }

        [Fact]
        public async Task GetAllCoursesAsync_HappyPath_ReturnsOkObjectResult()
        {
            //arrange
            courseServiceMock
                .Setup(m => m.GetCoursesAsync())
                .ReturnsAsync(new List<Course>()
                {
                    dummyCourse,
                    new Course(1, "testName1", "testDescription1", false),
                    new Course(2, "testName2", "testDescription2", true),
                    new Course(3, "testName3", "testDescription3", false)
                });

            //act
            var result = await courseController.GetAllCoursesAsync();

            //assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result, exactMatch: true);
            var dtos = Assert.IsType<IReadOnlyCollection<CourseResponseDTO>>(okObjectResult.Value, exactMatch: false);
            Assert.Equal(4, dtos.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task GetCourseAsync_HappyPath_ReturnsOkObjectResult(int courseId)
        {

            //arrange
            courseServiceMock
                .Setup(m => m.GetCourseAsync(It.IsAny<int>()))
                .ReturnsAsync(dummyCourse);

            //act
            var result = await courseController.GetCourseAsync(courseId);

            //assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CourseResponseDTO>(okObjectResult.Value);
            Assert.Equal(dummyCourse.Name, dto.Name);
            Assert.Equal(dummyCourse.Description, dto.Description);
            Assert.Equal(dummyCourse.IsDeprecated, !dto.IsActive);
        }

        [Theory]
        [InlineData(-1)]
        public async Task GetCourseAsync_InvalidCourseId_ReturnsBadRequest(int courseId)
        {
            //arrange 

            //act
            var result = await courseController.GetCourseAsync(courseId);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }


        [Theory]
        [InlineData(10)]
        public async Task GetCourseAsync_NotExistantCourse_ReturnsNotFound(int courseId)
        {
            //arrange
            courseServiceMock
                .Setup(m => m.GetCourseAsync(It.IsAny<int>()))
                .ReturnsAsync((Course?)null);

            //act
            var result = await courseController.GetCourseAsync(courseId);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCourseAsync_HappyPath_ReturnsCreatedAtRoute()
        {
            //arrange
            CourseRequestDTO courseRequestDTO = new CourseRequestDTO()
            {
                Name = dummyCourse.Name,
                Description = dummyCourse.Description,
            };

            courseServiceMock
                .Setup(m => m.CreateCourseAsync(It.IsAny<Course>()))
                .ReturnsAsync(dummyCourse.CourseId);

            //act
            var result = await courseController.CreateCourseAsync(courseRequestDTO);

            //assert
            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(nameof(CoursesController.GetCourseAsync), objectResult.RouteName);
            Assert.NotNull(objectResult.RouteValues);
            Assert.Single(objectResult.RouteValues);
            Assert.NotNull(objectResult.RouteValues.GetValueOrDefault("courseId"));
            Assert.IsType<CourseResponseDTO>(objectResult.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task UpdateCourseAsync_HappyPath_ReturnsNoContent(int courseId)
        {
            //arrange
            courseServiceMock
                .Setup(m => m.GetCourseAsync(courseId))
                .ReturnsAsync(dummyCourse);


            var courseRequestDTO = new CourseRequestDTO()
            {
                Name = dummyCourse.Name,
                Description = dummyCourse.Description
            };

            //act
            var result = await courseController.UpdateCourseAsync(courseId, courseRequestDTO);

            //assert
            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task UpdateCourseAsync_CourseNotExists_ReturnsCreatedAtRoute(int courseId)
        {
            courseServiceMock
                .Setup(m => m.GetCourseAsync(courseId))
                .ReturnsAsync(dummyCourse);

            courseServiceMock
                .Setup(m => m.CreateCourseAsync(It.IsAny<Course>()))
                .ReturnsAsync(dummyCourse.CourseId);

            //todo
        }
    }
}