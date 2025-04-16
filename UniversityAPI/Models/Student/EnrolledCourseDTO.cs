using Domain.Models.VObject;

namespace UniversityAPI.Models.Student
{
    public class EnrolledCourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTimeRange DateTimeRange { get; set; }
    }
}