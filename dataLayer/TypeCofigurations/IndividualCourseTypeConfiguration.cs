using dataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dataLayer.TypeCofigurations
{
    public class IndividualCourseTypeConfiguration : IEntityTypeConfiguration<IndividualCourse>
    {
        public void Configure(EntityTypeBuilder<IndividualCourse> builder)
        {

        }
    }
}