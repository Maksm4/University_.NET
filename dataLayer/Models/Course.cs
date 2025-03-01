namespace dataLayer.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deprecated { get; set; }
        public IList<IndividualCourse> individualCourses { get; set; }
        public IList<CourseModule> courseModules { get; set; }
    }
}