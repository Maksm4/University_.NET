namespace dataLayer.Models
{
    public class CourseModule
    {
        public int CourseModuleId { get; set; }
        public int CourseId { get; set; }
        public string Description { get; set; }
        public Course Course { get; set; }
        public IList<ModuleMark> moduleMarks { get; set; }
    }
}