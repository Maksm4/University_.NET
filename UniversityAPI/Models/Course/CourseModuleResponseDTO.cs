using Newtonsoft.Json;

namespace UniversityAPI.Models.Course
{
    public class CourseModuleResponseDTO
    {
        [JsonProperty("ModuleId")]
        public int CourseModuleId { get; set; }
        public string Description { get; set; }
    }
}