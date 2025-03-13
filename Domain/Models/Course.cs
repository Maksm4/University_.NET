using Domain.Models.Aggregate;

namespace Domain.Models
{
    public class Course : BaseEntity, IAggregateRoot
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deprecated { get; set; }
        private readonly List<CourseModule> _courseModules = new List<CourseModule>();
        public IEnumerable<CourseModule> CourseModules => _courseModules.AsReadOnly();

        public bool IsDeprecated => Deprecated;

        private Course() { }

        public Course(int courseId, string name, string description, bool deprecated)
        {
            CourseId = courseId;
            Name = name;
            Description = description;
            Deprecated = deprecated;
        }

    }
}