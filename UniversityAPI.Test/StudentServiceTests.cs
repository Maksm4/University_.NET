using ApplicationCore.DTO;
using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Service;
using Domain.Models;
using Domain.Models.Aggregate;
using Moq;

namespace UniversityAPI.Test
{
    public class StudentServiceTests
    {
        private Mock<IStudentRepository> studentRepositoryMock;
        private Mock<ICourseRepository> courseRepositoryMock;
        private IStudentService studentService;
        private Student dummyStudent;

        public StudentServiceTests()
        {
            studentRepositoryMock = new Mock<IStudentRepository>();
            courseRepositoryMock = new Mock<ICourseRepository>();
            studentService = new StudentService(studentRepositoryMock.Object, courseRepositoryMock.Object);

            dummyStudent = new Student("testname", "testlastname", DateOnly.FromDateTime(DateTime.Now), new LearningPlan("testname"));
            
        }

        [Fact]
        public async Task GetEnrolledCoursesWithGradesAsync_HappyPath_ReturnsListOfStudentCourseTakenDTO()
        {
            //arrange
            int studentId = 1;
            studentRepositoryMock
                .Setup(m => m.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(dummyStudent);

            courseRepositoryMock
                .Setup(m => m.GetCoursesAsync())
                .ReturnsAsync(new List<Course>());

            //act
            var result = await studentService.GetEnrolledCoursesWithGradesAsync(studentId);

            //assert
            Assert.NotNull(result);
            Assert.IsType<IReadOnlyCollection<StudentCourseTakenDTO>>(result, exactMatch: false);
        }

        [Fact]
        public async Task GetEnrolledCoursesWithGradesAsync_NotExistantStudent_ReturnsEmptyList()
        {
            //arrange
            int notExistantStudentId = 1;
            studentRepositoryMock
                .Setup(m => m.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Student?)null);

            //act
            var result = await studentService.GetEnrolledCoursesWithGradesAsync(notExistantStudentId);

            //assert
            Assert.NotNull(result);
            Assert.IsType<IReadOnlyCollection<StudentCourseTakenDTO>>(result, exactMatch: false);
            Assert.Empty(result);
        }
    }
}