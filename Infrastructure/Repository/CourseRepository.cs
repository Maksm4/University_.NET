using ApplicationCore.Context;
using ApplicationCore.GenericRepositories;
using ApplicationCore.IRepository;
using AutoMapper;
using Domain.Models;
using Domain.Models.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CourseRepository : CRUDRepository<Course>, ICourseRepository
    {
        private readonly UniversityContext dbContext;
        private readonly IMapper mapper;
        public CourseRepository(UniversityContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesWithModules()
        {
            var entityCourses = await dbContext.Courses
                .Include(c => c.CourseModules)
                .ThenInclude(cm => cm.ModuleMarks)
                .Include(c => c.IndividualCourses)
                .ToListAsync();

            return mapper.Map<IEnumerable<Course>>(entityCourses);
        }

        public async Task<Course?> GetCourseInfo(int courseId)
        {
            var entityCourse = await dbContext.Courses
                .Include(c => c.IndividualCourses)
                .Include(c => c.CourseModules)
                .FirstOrDefaultAsync(c=> c.CourseId == courseId);

            return mapper.Map<Course?>(entityCourse);
        }

        public async Task<IEnumerable<Course>> GetActiveCourses()
        {
            var entityCourses = await dbContext.Courses
                .Where(c => c.IndividualCourses.Any(ic => ic.StartDate != null))
                .ToListAsync();

            return mapper.Map<IEnumerable<Course>>(entityCourses);
        }
    }
}