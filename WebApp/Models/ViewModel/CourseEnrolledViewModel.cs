using Domain.Models.VObject;

namespace WebApp.Models.ViewModel
{
    public class CourseEnrolledViewModel : BaseCourseViewModel
    {
        public int StudentId { get; set; }
        public DateTimeRange DateTimeRange { get; set; }
    }
}