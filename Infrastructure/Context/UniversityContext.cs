using ApplicationCore.TypeCofigurations;
using Domain.Models.Aggregate;
using Infrastructure.TypeCofigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //config of tables

            modelBuilder.ApplyConfiguration(new CourseModuleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EnrolledCourseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LearningPlanTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MarkedModuleTypeConfiguration());
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