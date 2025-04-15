namespace UniversityAPI.Models
{
    public class CourseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public IReadOnlyCollection<CourseModuleDTO> courseModuleDTOs { get; set; }
    }
}