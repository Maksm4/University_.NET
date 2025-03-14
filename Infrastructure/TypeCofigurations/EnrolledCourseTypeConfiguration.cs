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

            builder.HasKey(ec => new { ec.CourseId, ec.LearningPlanId })
                .HasName("PK_EnrolledCourse");

            builder.HasOne<Course>()
                .WithMany()
                .HasForeignKey(ec => ec.CourseId);

            builder.HasMany(ec => ec.MarkedModules)
                .WithOne()
                .HasForeignKey("LearningPlanId", "CourseId");

            builder.OwnsOne(ec => ec.DateTimeRange, timeRange =>
            {
                timeRange.Property(ts => ts.StartTime)
                .HasColumnName("StartDate")
                .HasColumnType("date")
                .IsRequired();

                timeRange.Property(ts => ts.EndTime)
                .HasColumnName("EndDate")
                .HasColumnType("date");
            });
        }
    }
}