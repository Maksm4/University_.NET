using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class ModuleMarkTypeConfiguration : IEntityTypeConfiguration<ModuleMarkEntity>
    {
        public void Configure(EntityTypeBuilder<ModuleMarkEntity> builder)
        {
            builder.ToTable("ModuleMark");

            builder.HasKey(mm => new { mm.CourseModuleId, mm.StudentId })
                .HasName("PK_ModuleMark");

            builder.Property(mm => mm.Mark)
                .IsRequired();
        }
    }
}