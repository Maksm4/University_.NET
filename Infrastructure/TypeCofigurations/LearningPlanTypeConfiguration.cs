using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.TypeCofigurations
{
    public class LearningPlanTypeConfiguration : IEntityTypeConfiguration<LearningPlan>
    {
        public void Configure(EntityTypeBuilder<LearningPlan> builder)
        {
            builder.ToTable("LearningPlan");

            builder.HasKey(lp => lp.LearningPlanId)
                .HasName("PK_LearningPlan");

            builder.Property(lp => lp.LearningPlanId)
                .ValueGeneratedOnAdd();

            builder.HasMany(lp => lp.EnrolledCourses)
                .WithOne()
                .HasForeignKey(ic => ic.LearningPlanId)
                .IsRequired();

            builder.Property(lp => lp.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}