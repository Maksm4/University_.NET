using ApplicationCore.IRepository;
using Domain.Models;
using Domain.Models.Aggregate;
using Infrastructure.Context;
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

        public override async Task<IReadOnlyCollection<Course>> FindAllAsync()
        {
            return await dbContext.Courses
                .Include(c => c.CourseModules)
                .ToListAsync();
        }

        public override async Task<Course?> FindByIdAsync(int courseId)
        {
            return await dbContext.Courses
                .Include(c => c.CourseModules)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<IReadOnlyCollection<Course>> GetActiveCoursesAsync()
        {
            return await dbContext.Courses
               .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Course>> GetActiveCoursesAsync(IEnumerable<int> courseIds)
        {
            return await dbContext.Courses
                .Where(c => courseIds.Contains(c.CourseId))
                .ToListAsync();
        }
    }
}