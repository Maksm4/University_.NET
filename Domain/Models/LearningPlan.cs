using Domain.Models.Aggregate;

namespace Domain.Models
{
    public class LearningPlan
    {
        public int LearningPlanId { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        private readonly IList<EnrolledCourse> _enrolledCourses = new List<EnrolledCourse>();
        public IReadOnlyCollection<EnrolledCourse> EnrolledCourses => _enrolledCourses.AsReadOnly();

        private LearningPlan() { }
        public LearningPlan(string name, int studentId)
        {
            Name = name;
            StudentId = studentId;
        }

        public void AddCourse(EnrolledCourse course)
        {
            _enrolledCourses.Add(course);
        }

        public EnrolledCourse? GetEnrolledCourse(int courseId)
        {
            return EnrolledCourses.FirstOrDefault(ec => ec.CourseId == courseId);
        }
    }
}