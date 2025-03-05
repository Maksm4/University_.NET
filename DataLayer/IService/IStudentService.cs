using ApplicationCore.Models;
using ApplicationCore.Models.ValueObject;

namespace ApplicationCore.IService
{
    public interface IStudentService
    {
        Task<Student?> GetStudentInfo(int studentId);
        Task<IEnumerable<ModuleMark>> GetModuleMarks(int studentId);
        Task CreateLearningPlan(LearningPlan learningPlan, Student student);
        Task<IEnumerable<Student>> GetAllStudentsWithLearningPlans(int studentId);
        Task<IEnumerable<StudentGrade>> GetStudentsWithTheirAverageGrade();
        Task UpdateStudentLearningPlan(LearningPlan learningPlan, Student student);
    }
}