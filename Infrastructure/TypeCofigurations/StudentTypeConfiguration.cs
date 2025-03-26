using Domain.Models;
using Domain.Models.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.TypeCofigurations
{
    public class StudentTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student");

            builder.HasKey(s => s.StudentId)
                .HasName("PK_Student");

            builder.Property(s => s.StudentId)
                .ValueGeneratedOnAdd();

            builder.HasOne(s => s.LearningPlan)
                .WithOne()
                .HasForeignKey<LearningPlan>(lp => lp.StudentId)
                .IsRequired();

            builder.Property(s => s.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(s => s.BirthDate)
                .HasColumnType("date");

            //make sure it works as its using valueobject from domain models
            //builder.OwnsOne(s => s.Email, email =>
            //{
            //    email.Property(e => e.Address)
            //     .HasColumnName("Email")
            //     .HasMaxLength(1000)
            //     .IsRequired();
            //});
        }
    }
}