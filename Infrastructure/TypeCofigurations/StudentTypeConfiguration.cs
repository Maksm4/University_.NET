using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationCore.TypeCofigurations
{
    public class StudentTypeConfiguration : IEntityTypeConfiguration<StudentEntity>
    {
        public void Configure(EntityTypeBuilder<StudentEntity> builder)
        {
            builder.ToTable("Student");

            builder.HasKey(s => s.StudentId)
                .HasName("PK_Student");

            builder.HasMany(s => s.ModuleMarks)
                .WithOne(mm => mm.Student)
                .HasForeignKey(s => s.StudentId)
                .IsRequired();

            builder.HasOne(s => s.LearningPlan)
                .WithOne(lp => lp.Student)
                .HasForeignKey<LearningPlanEntity>(lp => lp.StudentId)
                .IsRequired();

            builder.Property(s => s.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.Email)
                .HasMaxLength(1000)
                .IsRequired();
        }
    }
}