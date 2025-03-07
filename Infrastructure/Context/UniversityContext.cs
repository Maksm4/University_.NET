using ApplicationCore.TypeCofigurations;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Context
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        public DbSet<CourseEntity> Courses { get; set; }
        public DbSet<CourseModuleEntity> CourseModules { get; set; }
        public DbSet<IndividualCourseEntity> IndividualCourses { get; set; }
        public DbSet<LearningPlanEntity> LearningPlans { get; set; }
        public DbSet<ModuleMarkEntity> ModuleMarks { get; set; }
        public DbSet<StudentEntity> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //config of tables

            modelBuilder.ApplyConfiguration(new CourseModuleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IndividualCourseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LearningPlanTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleMarkTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StudentTypeConfiguration());

            //some initial data

            //modelBuilder.Entity<Student>().HasData(
            //        new Student { FirstName = "max", LastName = "Moszynski",  }                
            //    );

            //modelBuilder.Entity<Course>().HasData(
            //        new Course { Name = "databases", Description = "designing databases in vertabelo", Deprecated = false},
            //        new Course { Name = "ML", Description = "intro to ml, pytorch fundamentals", Deprecated = true}
            //    );

            //modelBuilder.Entity<CourseModule>().HasData(

            //    );
        }
    }
}