using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.TypeCofigurations
{
    public class MarkedModuleTypeConfiguration : IEntityTypeConfiguration<MarkedModule>
    {
        public void Configure(EntityTypeBuilder<MarkedModule> builder)
        {
            builder.ToTable("ModuleMark");

            builder.HasKey(mm => new { mm.CourseModuleId, mm.CourseId, mm.LearningPlanId })
                .HasName("PK_ModuleMark");

            builder.HasOne<CourseModule>()
                .WithMany()
                .HasForeignKey(mm => mm.CourseModuleId);

            builder.Property(mm => mm.Mark)
                .IsRequired();
        }
    }
}