namespace Domain.Models
{
    public class LearningPlan : BaseEntity
    {
        public int LearningPlanId { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        public ICollection<EnrolledCourse> EnrolledCourses { get; set; } = new List<EnrolledCourse>();

        public void AddCourse(EnrolledCourse course)
        {
            EnrolledCourses.Add(course);
        }

        public EnrolledCourse? GetEnrolledCourse(Course course)
        {
            return EnrolledCourses.FirstOrDefault(ec => ec.CourseId == course.CourseId);
        }
    }
}