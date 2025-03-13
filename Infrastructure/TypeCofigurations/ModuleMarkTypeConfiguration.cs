using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class ModuleMarkTypeConfiguration : IEntityTypeConfiguration<MarkedModule>
    {
        public void Configure(EntityTypeBuilder<MarkedModule> builder)
        {
            builder.ToTable("ModuleMark");

            builder.HasKey(mm => new { mm.CourseModuleId, mm.StudentId })
                .HasName("PK_ModuleMark");

            builder.Property(mm => mm.Mark)
                .IsRequired();
        }
    }
}