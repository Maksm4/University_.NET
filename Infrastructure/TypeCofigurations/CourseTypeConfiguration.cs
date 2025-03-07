using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class CourseTypeConfiguration : IEntityTypeConfiguration<CourseEntity>
    {
        public void Configure(EntityTypeBuilder<CourseEntity> builder)
        {
            builder.ToTable("Course");

            builder.HasKey(c => c.CourseId)
                .HasName("PK_Course");

            builder.HasMany(c => c.IndividualCourses)
                 .WithOne(ic => ic.Course)
                 .HasForeignKey(ic => ic.CourseId)
                 .IsRequired();

            builder.HasMany(c => c.CourseModules)
                .WithOne(cm => cm.Course)
                .HasForeignKey(cm => cm.CourseId)
                .IsRequired();

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(1000)
                .IsRequired();
        }
    }
}