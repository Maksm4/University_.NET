using dataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataLayer.TypeCofigurations
{
    public class CourseModuleTypeConfiguration : IEntityTypeConfiguration<CourseModule>
    {
        public void Configure(EntityTypeBuilder<CourseModule> builder)
        {
            builder.HasMany(cm => cm.ModuleMarks)
                .WithOne(mm => mm.CourseModule)
                .HasForeignKey(mm => mm.CourseModuleId)
                .IsRequired();

            builder.Property(cm => cm.Description).HasColumnName("Description");
        }
    }
}