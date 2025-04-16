namespace UniversityAPI.Models
{
    public class CourseResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public IReadOnlyCollection<CourseModuleResponseDTO> courseModuleDTOs { get; set; }
    }
}