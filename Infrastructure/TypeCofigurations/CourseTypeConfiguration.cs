using Domain.Models;
using Domain.Models.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class CourseTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Course");

            builder.HasKey(c => c.CourseId)
                .HasName("PK_Course");

            builder.Property(c => c.CourseId)   
                .ValueGeneratedOnAdd();

            builder.HasMany(c => c.CourseModules)
                .WithOne()
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