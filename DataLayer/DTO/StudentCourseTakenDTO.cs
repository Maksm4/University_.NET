using Domain.Models.VObject;

namespace ApplicationCore.DTO
{
    public class StudentCourseTakenDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTimeRange DateTimeRange { get; set; }
        public IReadOnlyCollection<CourseModuleWithMarkDTO> courseModuleWithMarkDTOs { get; set; }
    }
}