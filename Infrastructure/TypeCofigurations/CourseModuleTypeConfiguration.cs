using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.TypeCofigurations
{
    public class CourseModuleTypeConfiguration : IEntityTypeConfiguration<CourseModule>
    {
        public void Configure(EntityTypeBuilder<CourseModule> builder)
        {
            builder.ToTable("CourseModule");

            builder.HasKey(cm => cm.CourseModuleId)
                .HasName("PK_CourseModule");

            builder.Property(cm => cm.CourseModuleId)
                .ValueGeneratedOnAdd();

            builder.Property(cm => cm.Description)
                .HasMaxLength(1000)
                .IsRequired();
        }
    }
}