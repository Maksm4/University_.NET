using Domain.Models;
using Domain.Models.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.TypeCofigurations
{
    public class EnrolledCourseTypeConfiguration : IEntityTypeConfiguration<EnrolledCourse>
    {
        public void Configure(EntityTypeBuilder<EnrolledCourse> builder)
        {
            builder.ToTable("EnrolledCourse");

            builder.HasKey(ic => new { ic.CourseId, ic.LearningPlanId })
                .HasName("PK_EnrolledCourse");

            builder.HasOne<Course>()
                .WithMany()
                .HasForeignKey(ec => ec.CourseId);

            builder.HasMany(ec => ec.MarkedModules)
                .WithOne()
                .HasForeignKey(mm => mm.EnrolledCourseId);
        }
    }
}