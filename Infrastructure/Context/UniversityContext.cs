using ApplicationCore.TypeCofigurations;
using Domain.Models;
using Domain.Models.Aggregate;
using Domain.Models.VObject;
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
            //toogle off default delete cascades

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfiguration(new CourseModuleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EnrolledCourseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LearningPlanTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MarkedModuleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StudentTypeConfiguration());
        }
    }
}