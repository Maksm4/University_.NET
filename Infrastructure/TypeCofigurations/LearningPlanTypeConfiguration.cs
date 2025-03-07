using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class LearningPlanTypeConfiguration : IEntityTypeConfiguration<LearningPlanEntity>
    {
        public void Configure(EntityTypeBuilder<LearningPlanEntity> builder)
        {
            builder.ToTable("LearningPlan");

            builder.HasKey(lp => lp.LearningPlanId)
                .HasName("PK_LearningPlan");

            builder.HasMany(lp => lp.IndividualCourses)
                .WithOne(ic => ic.LearningPlan)
                .HasForeignKey(ic => ic.LearningPlanId)
                .IsRequired();

            builder.Property(lp => lp.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}