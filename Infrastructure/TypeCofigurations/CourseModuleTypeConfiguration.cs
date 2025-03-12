using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class CourseModuleTypeConfiguration : IEntityTypeConfiguration<CourseModule>
    {
        public void Configure(EntityTypeBuilder<CourseModule> builder)
        {
            builder.ToTable("CourseModule");

            builder.HasMany(cm => cm.ModuleMarks)
                .WithOne(mm => mm.CourseModule)
                .HasForeignKey(mm => mm.CourseModuleId)
                .IsRequired();

            builder.HasKey(cm => cm.CourseModuleId)
                .HasName("PK_CourseModule");

            builder.Property(cm => cm.Description)
                .HasMaxLength(1000)
                .IsRequired();

        }
    }
}