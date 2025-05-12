using Newtonsoft.Json;

namespace UniversityAPI.Models.Course
{
    public class CourseResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [JsonProperty("modules")]
        public IReadOnlyCollection<CourseModuleResponseDTO> CourseModuleDTOs { get; set; }
    }
}