using dataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataLayer.TypeCofigurations
{
    public class CourseTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasMany(c => c.IndividualCourses)
                 .WithOne(ic => ic.Course)
                 .HasForeignKey(ic => ic.CourseId)
                 .IsRequired();
            
            builder.HasMany(c => c.CourseModules)
                .WithOne(cm => cm.Course)
                .HasForeignKey(cm => cm.CourseId)
                .IsRequired();
        }
    }
}