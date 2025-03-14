using ApplicationCore.Context;
using ApplicationCore.IRepository;
using Domain.Models.Aggregate;
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

        public async Task<IEnumerable<Student>> GetAllStudentsWithEnrolledCourses()
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ThenInclude(lp => lp.EnrolledCourses)
                .ToListAsync();
        }

        public override async Task<Student?> FindById(int studentId)
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ThenInclude(lp => lp.EnrolledCourses)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }
    }
}