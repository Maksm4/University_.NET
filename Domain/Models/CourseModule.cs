namespace Domain.Models
{
    public class CourseModule : BaseEntity
    {
        public int CourseModuleId { get; set; }
        public int CourseId { get; set; }
        public string Description { get; set; }

        private CourseModule() { }

        public CourseModule(int courseModuleId, int courseId, string description)
        {
            CourseModuleId = courseModuleId;
            CourseId = courseId;
            Description = description;
        }
    }
}