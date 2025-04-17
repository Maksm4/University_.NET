namespace UniversityAPI.Models.Student
{
    public class EnrolledCourseRequestDTO
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}