using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class IndividualCourseTypeConfiguration : IEntityTypeConfiguration<EnrolledCourse>
    {
        public void Configure(EntityTypeBuilder<EnrolledCourse> builder)
        {
            builder.ToTable("IndividualCourse");

            builder.HasKey(ic => new { ic.CourseId, ic.LearningPlanId })
                .HasName("PK_IndividualCourse");

            builder.Property(ic => ic.StartDate)
                .HasColumnType("date");

            builder.Property(ic => ic.EndDate)
                .HasColumnType("date");
        }
    }
}