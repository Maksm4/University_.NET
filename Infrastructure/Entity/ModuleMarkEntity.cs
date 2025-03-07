namespace Infrastructure.Entity
{
    public class ModuleMarkEntity
    {
        public int CourseModuleId { get; set; }
        public CourseModuleEntity CourseModule { get; set; }
        public int StudentId { get; set; }
        public StudentEntity Student { get; set; }
        public int Mark { get; set; }
    }
}