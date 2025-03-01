using dataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataLayer.TypeCofigurations
{
    public class LearningPlanTypeConfiguration : IEntityTypeConfiguration<LearningPlan>
    {
        public void Configure(EntityTypeBuilder<LearningPlan> builder)
        {
            builder.HasMany(lp => lp.IndividualCourses)
                .WithOne(ic => ic.LearningPlan)
                .HasForeignKey(ic => ic.LearningPlanId)
                .IsRequired();
        }
    }
}