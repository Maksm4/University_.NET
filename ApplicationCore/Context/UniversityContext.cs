using dataLayer.Models;
using dataLayer.TypeCofigurations;
using Microsoft.EntityFrameworkCore;

namespace dataLayer.Context
{
    public class UniversityContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseModule> CourseModules { get; set; }
        public DbSet<IndividualCourse> IndividualCourses { get; set; }
        public DbSet<LearningPlan> LearningPlans { get; set; }
        public DbSet<ModuleMark> ModuleMarks { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "connStr"
                );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new CourseTypeConfiguration().Configure(modelBuilder.Entity<Course>());
            new CourseModuleTypeConfiguration().Configure(modelBuilder.Entity<CourseModule>());
            new IndividualCourseTypeConfiguration().Configure(modelBuilder.Entity<IndividualCourse>());
            new LearningPlanTypeConfiguration().Configure(modelBuilder.Entity<LearningPlan>());
            new ModuleMarkTypeConfiguration().Configure(modelBuilder.Entity<ModuleMark>());
            new StudentTypeConfiguration().Configure(modelBuilder.Entity<Student>());
        }
    }
}