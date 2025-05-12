using Newtonsoft.Json;

namespace UniversityAPI.Models.EnrolledCourse
{
    public class EnrolledCourseResponseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public bool IsActive { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        [JsonProperty("gradedModules")]
        public IReadOnlyCollection<MarkedCourseModuleResponseDTO> markedCourseModules { get; set; }
    }
}