namespace UniversityAPI.Models.EnrolledCourse
{
    public class MarkedCourseModuleResponseDTO
    {
        public int? Mark { get; set; }
        public int CourseModuleId { get; set; }
        public required string Description { get; set; }
    }
}