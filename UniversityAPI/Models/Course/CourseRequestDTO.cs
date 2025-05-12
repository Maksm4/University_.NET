using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models.Course
{
    public class CourseRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}