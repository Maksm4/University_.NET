using ApplicationCore.Context;
using ApplicationCore.IRepository;
using AutoMapper;
using Domain.Models;
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

        public async Task<IEnumerable<Student>> GetAllStudentsWithLearningPlans()
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ToListAsync();
        }

        public override async Task<Student?> FindById(int studentId)
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ThenInclude(lp => lp.IndividualCourses)
                .Include(s => s.ModuleMarks)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }
    }
}