namespace Infrastructure.Entity
{
    public class CourseModuleEntity
    {
        public int CourseModuleId { get; set; }
        public int CourseId { get; set; }
        public CourseEntity Course { get; set; }
        public string Description { get; set; }
        public ICollection<ModuleMarkEntity> ModuleMarks { get; set; } = new List<ModuleMarkEntity>();
    }
}