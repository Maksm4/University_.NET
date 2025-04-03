using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.TypeCofigurations
{
    internal class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasOne(u => u.student)
                .WithOne()
                .HasForeignKey<User>(u => u.studentId);

            builder.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}