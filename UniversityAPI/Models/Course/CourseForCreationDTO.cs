using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models
{
    public class CourseForCreationDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}