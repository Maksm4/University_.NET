using Domain.Models.VObject;

namespace WebApp.Models.ViewModel
{
    public class CourseEnrolledViewModel
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTimeRange DateTimeRange { get; set; }
    }
}
