using ApplicationCore.Context;
using ApplicationCore.GenericRepositories;
using ApplicationCore.IRepository;
using ApplicationCore.Models;
using ApplicationCore.Models.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class StudentRepository : CRUDRepository<Student>, IStudentRepository
    {
        private readonly UniversityContext dbContext;
        public StudentRepository(UniversityContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateLearningPlan(LearningPlan learningPlan, Student student)
        {
            student.LearningPlan = learningPlan;
            await dbContext.LearningPlans.AddAsync(learningPlan);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsWithLearningPlans()
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ToListAsync();
        }

        public async Task<IEnumerable<ModuleMark>> GetModuleMarks(int studentId)
        {
            return await dbContext.ModuleMarks
                .Where(mm => mm.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ModuleMark>> GetStudentmarks(int studentId)
        {
            return await dbContext.ModuleMarks
                .Where(mm => mm.StudentId == studentId)
                .Include(mm => mm.CourseModule)
                .Include(mm => mm.Student)
                .ToListAsync();
        }
        
        public async Task<Student?> GetStudentInfo(int studentId)
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ThenInclude(lp => lp.IndividualCourses)
                .Include(s => s.ModuleMarks)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

        }

        public async Task<IEnumerable<StudentGrade>> GetStudentsWithTheirAverageGrade()
        {
            return await dbContext.Students.Select(s => new StudentGrade
            {
                Student = s,
                Grade = s.ModuleMarks.Average(mm => mm.Mark)
            }).ToListAsync();
        }

        public async Task UpdateStudentLearningPlan(LearningPlan learningPlan, Student student)
        {
            student.LearningPlan = learningPlan;
            dbContext.Students.Update(student);
        }
    }
}