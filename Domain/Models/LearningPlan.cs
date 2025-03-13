using Domain.Models.Aggregate;

namespace Domain.Models
{
    public class LearningPlan : BaseEntity
    {
        public int LearningPlanId { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        private readonly IList<EnrolledCourse> _enrolledCourses = new List<EnrolledCourse>();
        public IEnumerable<EnrolledCourse> EnrolledCourses => _enrolledCourses.AsReadOnly();

        public void AddCourse(EnrolledCourse course)
        {
            _enrolledCourses.Add(course);
        }

        public EnrolledCourse? GetEnrolledCourse(Course course)
        {
            return EnrolledCourses.FirstOrDefault(ec => ec.CourseId == course.CourseId);
        }
    }
}