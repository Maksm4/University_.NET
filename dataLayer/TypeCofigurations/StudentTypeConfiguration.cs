using dataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataLayer.TypeCofigurations
{
    public class StudentTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasMany(s => s.ModuleMarks)
                .WithOne(mm => mm.Student)
                .HasForeignKey(s => s.StudentId)
                .IsRequired();

            builder.HasOne(s => s.LearningPlan)
                .WithOne(lp => lp.Student)
                .HasForeignKey<LearningPlan>(lp => lp.StudentId)
                .IsRequired();
        }
    }
}