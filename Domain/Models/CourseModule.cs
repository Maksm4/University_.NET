namespace Domain.Models
{
    public class CourseModule : BaseEntity
    {
        public int CourseModuleId { get; set; }
        public int CourseId { get; set; }
        public string Description { get; set; }
    }
}