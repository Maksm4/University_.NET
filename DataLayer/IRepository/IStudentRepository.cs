using DataLayer.GenericRepositories;
using DataLayer.Models;

namespace DataLayer.IRepository
{
    public interface IStudentRepository : ICRUDRepository<Student>
    {
        Task<Student> GetStudentInfo(int studentId);
        Task<LearningPlan> GetLearningPlan(int studentId);
        Task<IList<ModuleMark>> GetModuleMarks(int studentId);
        Task CreateLearningPlan(LearningPlan learningPlan, int studentId);
        Task<IList<Student>> GetAllStudentsWithLearningPlans(int studentId);
        Task UpdateStudentLearningPlan(LearningPlan learningPlan, int studentId);
        Task RemoveStudentFromCourse(int courseId, int studentId);
    }
}