namespace dataLayer.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deprecated { get; set; }
        public ICollection<IndividualCourse> individualCourses { get; } = new List<IndividualCourse>();
        public ICollection<CourseModule> courseModules { get; } = new List<CourseModule>();
    }
}