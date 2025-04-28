using ApplicationCore.IService;
using AutoMapper;
using Domain.Models.Aggregate;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR.Protocol;
using Moq;
using Newtonsoft.Json;
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
            Assert.IsType<CourseResponseDTO>(okObjectResult.Value);
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

        [Fact]
        public async Task CreateCourseAsync_NullInputParam_ReturnsBadRequest()
        {
            //act
            var result = await courseController.CreateCourseAsync(null!);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateCourseAsync_PartialUpdate_ReturnsBadRequest()
        {
            //arrange
            var requestDTO = new CourseRequestDTO()
            {
                Name = dummyCourse.Name
            };

            //act
            var result = await courseController.CreateCourseAsync(requestDTO);

            //assert
            Assert.IsType<BadRequestResult>(result);
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
            //arrange
            courseServiceMock
                .Setup(m => m.GetCourseAsync(courseId))
                .ReturnsAsync((Course?)null);

            courseServiceMock
                .Setup(m => m.CreateCourseAsync(It.IsAny<Course>()))
                .ReturnsAsync(dummyCourse.CourseId);

            var courseRequestDTO = new CourseRequestDTO()
            {
                Name = dummyCourse.Name,
                Description = dummyCourse.Description
            };

            //act
            var result = await courseController.UpdateCourseAsync(courseId, courseRequestDTO);

            //assert

            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(nameof(CoursesController.GetCourseAsync), objectResult.RouteName);
            Assert.NotNull(objectResult.RouteValues);
            Assert.Single(objectResult.RouteValues);
            Assert.NotNull(objectResult.RouteValues.GetValueOrDefault("courseId"));
            Assert.IsType<CourseResponseDTO>(objectResult.Value);
        }

        [Theory]
        [InlineData(-1)]
        public async Task UpdateCourseAsync_WrongCourseId_ReturnsBadRequest(int courseid)
        {
            //arrange
            var courseRequestDTO = new CourseRequestDTO()
            {
                Name = dummyCourse.Name,
                Description = dummyCourse.Description
            };

            //act
            var result = await courseController.UpdateCourseAsync(courseid, courseRequestDTO);

            //assert
            var objectResult = Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task PartiallyUpdateCourseAsync_HappyPath_ReturnsNoContent(int courseId)
        {
            //arrange
            string replaceName = "replaceName";
            courseServiceMock
                .Setup(m => m.GetCourseAsync(courseId))
                .ReturnsAsync(dummyCourse);

            var patchDocument = new JsonPatchDocument<CourseRequestDTO>();
            patchDocument.Replace(crs => crs.Name, replaceName);

            //act
            var result = await courseController.PartiallyUpdateCourseAsync(courseId, patchDocument);

            //assert
            Assert.IsType<NoContentResult>(result);

            //not sure if this should be separated to another tesT?
            Assert.Equal(dummyCourse.Name, replaceName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task PartiallyUpdateCourseAsync_InvalidModelState_ReturnsBadReequestObject(int courseId)
        {
            //arrange
            courseServiceMock
                .Setup(m => m.GetCourseAsync(courseId))
                .ReturnsAsync(dummyCourse);

            var patchDocument = new JsonPatchDocument<CourseRequestDTO>();
            patchDocument.Replace(crs => crs.Name, "too long name");

            courseController.ModelState.AddModelError("Name", "too long name");

            //act
            var result = await courseController.PartiallyUpdateCourseAsync(courseId, patchDocument);

            //assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(objectResult.Value);
        }

        [Theory]
        [InlineData(-1)]
        public async Task PartiallyUpdateCourseAsync_InvalidInputParams_ReturnsBadRequest(int courseId)
        {
            //act
            var result = await courseController.PartiallyUpdateCourseAsync(courseId, null!);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PartiallyUpdateCourseAsync_CourseNotExists_ReturnNotFound()
        {
            //arrange
            int courseId = 10;
            courseServiceMock
              .Setup(m => m.GetCourseAsync(courseId))
              .ReturnsAsync((Course?)null);

            var patchDocument = new JsonPatchDocument<CourseRequestDTO>();

            //act
            var result = await courseController.PartiallyUpdateCourseAsync(courseId, patchDocument);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task DeleteCourseAsync_HappyPath_ReturnsNoContent(int courseId)
        {
            //arrange
            courseServiceMock
                .Setup(m => m.DeleteCourseAsync(courseId))
                .ReturnsAsync(true);

            //act
            var result = await courseController.DeleteCourseAsync(courseId);

            //assert
            Assert.IsType<NoContentResult>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task DeleteCourseAsync_WrongCourseId_ReturnBadRequest(int courseId)
        {
            //act
            var result = await courseController.DeleteCourseAsync(courseId);

            //assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCourseAsync_CourseNotExists_ReturnsNotFound()
        {
            //arrange
            courseServiceMock
                .Setup(m => m.DeleteCourseAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            //act
            var result = await courseController.DeleteCourseAsync(10);

            //assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}