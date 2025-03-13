namespace Domain.Models
{
    public class MarkedModule : BaseEntity
    {
        public int EnrolledCourseId { get; set; }
        public int CourseModuleId { get; set; }
        public int Mark { get; private set; }
        private MarkedModule() { }

        public MarkedModule(int enrolledCourseId, int courseModuleId, int mark)
        {
            EnrolledCourseId = enrolledCourseId;
            CourseModuleId = courseModuleId;
            if (mark < 3 || mark > 5)
            {
                throw new ArgumentException("mark has to be in 3-5 range");
            }
            Mark = mark;
        }
    }
}