using ApplicationCore.GenericRepositories;
using ApplicationCore.Models;
using ApplicationCore.Models.ValueObject;

namespace ApplicationCore.IRepository
{
    public interface IStudentRepository : ICRUDRepository<Student>
    {
        Task<Student?> GetStudentInfo(int studentId);
        Task<IEnumerable<ModuleMark>> GetModuleMarks(int studentId);
        Task CreateLearningPlan(LearningPlan learningPlan, Student student);
        Task<IEnumerable<Student>> GetAllStudentsWithLearningPlans(int studentId);
        Task<IEnumerable<StudentGrade>> GetStudentsWithTheirAverageGrade();
        Task UpdateStudentLearningPlan(LearningPlan learningPlan, Student student);
        public Task<IEnumerable<ModuleMark>> GetStudentmarks(int studentId);
    }
}