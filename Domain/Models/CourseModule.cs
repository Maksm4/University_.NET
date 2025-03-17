namespace Domain.Models
{
    public class CourseModule : BaseEntity
    {
        public int CourseModuleId { get; private set; }
        public int CourseId { get; private set; }
        public string Description { get; private set; }

        private CourseModule() { }

        internal CourseModule(int courseId, string description)
        {
            CourseId = courseId;
            Description = description;
        }
    }
}