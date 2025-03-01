using dataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataLayer.TypeCofigurations
{
    public class IndividualCourseTypeConfiguration : IEntityTypeConfiguration<IndividualCourse>
    {
        public void Configure(EntityTypeBuilder<IndividualCourse> builder)
        {
            builder.HasKey(ic => new { ic.CourseId, ic.LearningPlanId })
                .HasName("PK_IndividualCourse");

            builder.Property(ic => ic.StartDate)
                .HasColumnType("date");

            builder.Property(ic => ic.EndDate)
                .HasColumnType("date");
        }
    }
}