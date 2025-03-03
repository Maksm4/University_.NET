namespace DataLayer.Models
{
    public class CourseModule
    {
        public int CourseModuleId { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Description { get; set; }
        public ICollection<ModuleMark> ModuleMarks { get; set; } = new List<ModuleMark>();
    }
}