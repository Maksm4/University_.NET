using ApplicationCore.Context;
using ApplicationCore.GenericRepositories;
using ApplicationCore.IRepository;
using Domain.Models;
using Domain.Models.ValueObject;
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

        public async Task<IEnumerable<Course>> GetAllCoursesWithModules()
        {
            return await dbContext.Courses
                .Include(c => c.CourseModules)
                .ThenInclude(cm => cm.ModuleMarks)
                .Include(c => c.IndividualCourses)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCourses(int studentId)
        {
            return await dbContext.Courses
                .Include(c => c.IndividualCourses)
                .ThenInclude(ic => ic.LearningPlan)
                .ThenInclude(lp => lp.Student.StudentId == studentId)
                .ToListAsync();
                
        }

        public async Task<Course?> GetCourseInfo(int courseId)
        {
            return await dbContext.Courses
                .Include(c => c.IndividualCourses)
                .Include(c => c.CourseModules)
                .FirstOrDefaultAsync(c=> c.CourseId == courseId);
        }

        public async Task<IEnumerable<CourseCount>> GetCoursesWithModuleCount()
        {
            return await dbContext.Courses.Select(c => new CourseCount
           { 
                Course = c,
                ModuleCount = c.CourseModules.Count()
            
            }).ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetActiveCourses()
        {
            return await dbContext.Courses
                .Where(c => c.IndividualCourses.Any(ic => ic.StartDate != null))
                .ToListAsync();
        }
    }
}