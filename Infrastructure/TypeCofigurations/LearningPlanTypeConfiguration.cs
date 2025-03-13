using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class LearningPlanTypeConfiguration : IEntityTypeConfiguration<LearningPlan>
    {
        public void Configure(EntityTypeBuilder<LearningPlan> builder)
        {
            builder.ToTable("LearningPlan");

            builder.HasKey(lp => lp.LearningPlanId)
                .HasName("PK_LearningPlan");

            builder.HasMany(lp => lp.EnrolledCourses)
                .WithOne(ic => ic.LearningPlan)
                .HasForeignKey(ic => ic.LearningPlanId)
                .IsRequired();

            builder.Property(lp => lp.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}