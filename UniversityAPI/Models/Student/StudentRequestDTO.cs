using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models.Student
{
    public class StudentRequestDTO
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string LastName { get; set; }
        [Required]  
        public DateOnly BirthDate { get; set; }
    }
}