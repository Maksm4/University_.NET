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
        private readonly IMapper mapper;
        public StudentRepository(UniversityContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsWithLearningPlans()
        {
            var entityStudents = await dbContext.Students
                .Include(s => s.LearningPlan)
                .ToListAsync();

            return mapper.Map<IEnumerable<Student>>(entityStudents);
        }
 
        public override async Task<Student?> FindById(int studentId)
        {
            var entityStudent = await dbContext.Students
                .Include(s => s.LearningPlan)
                .ThenInclude(lp => lp.IndividualCourses)
                .Include(s => s.ModuleMarks)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            return mapper.Map<Student?>(entityStudent);
        }
    }
}