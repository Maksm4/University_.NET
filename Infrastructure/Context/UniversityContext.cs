using ApplicationCore.TypeCofigurations;
using Domain.Models.Aggregate;
using Infrastructure.TypeCofigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Context
{
    public class UniversityContext : IdentityDbContext<User>
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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

            modelBuilder.Entity<User>()
                .HasOne(u => u.student)
                .WithOne()
                .HasForeignKey<User>(u => u.studentId);
        }
    }
}