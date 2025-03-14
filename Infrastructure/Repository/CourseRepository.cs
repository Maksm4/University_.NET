using ApplicationCore.Context;
using ApplicationCore.IRepository;
using Domain.Models.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CourseRepository : CRUDRepository<Course>, ICourseRepository
    {
        private readonly UniversityContext dbContext;
        public CourseRepository(UniversityContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public override async Task<IEnumerable<Course>> FindAll()
        {
            return await dbContext.Courses
                .Include(c => c.CourseModules)
                .ToListAsync();
        }

        public override async Task<Course?> FindById(int courseId)
        {
            return await dbContext.Courses
                .Include(c => c.CourseModules)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> GetActiveCourses()
        {
            return await dbContext.Courses
               .Where(c => !c.Deprecated)
               .ToListAsync();
        }
    }
}