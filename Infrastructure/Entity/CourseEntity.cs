namespace Infrastructure.Entity
{
    public class CourseEntity
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deprecated { get; set; }
        public ICollection<IndividualCourseEntity> IndividualCourses { get; } = new List<IndividualCourseEntity>();
        public ICollection<CourseModuleEntity> CourseModules { get; } = new List<CourseModuleEntity>();
    }
}