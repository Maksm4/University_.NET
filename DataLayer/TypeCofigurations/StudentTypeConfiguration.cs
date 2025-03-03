using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.TypeCofigurations
{
    public class StudentTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId)
                .HasName("PK_Student");

            builder.HasMany(s => s.ModuleMarks)
                .WithOne(mm => mm.Student)
                .HasForeignKey(s => s.StudentId)
                .IsRequired();

            builder.HasOne(s => s.LearningPlan)
                .WithOne(lp => lp.Student)
                .HasForeignKey<LearningPlan>(lp => lp.StudentId)
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