namespace ApplicationCore.Models
{
    public class ModuleMark
    {
        public int CourseModuleId { get; set; }
        public CourseModule CourseModule { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int Mark { get; set; }
    }
}