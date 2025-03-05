using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class ModuleMarkTypeConfiguration : IEntityTypeConfiguration<ModuleMark>
    {
        public void Configure(EntityTypeBuilder<ModuleMark> builder)
        {
            builder.HasKey(mm => new { mm.CourseModuleId, mm.StudentId })
                .HasName("PK_ModuleMark");

            builder.Property(mm => mm.Mark)
                .IsRequired();
        }
    }
}