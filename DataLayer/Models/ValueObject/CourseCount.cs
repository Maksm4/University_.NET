namespace ApplicationCore.Models.ValueObject
{
    public class CourseCount
    {
        public required Course Course { get; set; }
        public int ModuleCount { get; set; }
    }
}