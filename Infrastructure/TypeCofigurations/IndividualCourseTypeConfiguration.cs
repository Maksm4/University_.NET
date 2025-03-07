using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class IndividualCourseTypeConfiguration : IEntityTypeConfiguration<IndividualCourseEntity>
    {
        public void Configure(EntityTypeBuilder<IndividualCourseEntity> builder)
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