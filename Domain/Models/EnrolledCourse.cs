using Domain.Models.ValueObject;

namespace Domain.Models
{
    public class EnrolledCourse : BaseEntity
    {
        public int LearningPlanId { get; set; }
        public int CourseId { get; set; }
        public DateTimeRange DateTimeRange { get; private set; }
        public ICollection<MarkedModule> moduleMarks { get; set; } = new List<MarkedModule>();

        public EnrolledCourse(int learningPlanId, int courseId, DateTimeRange dateTimeRange)
        {
            LearningPlanId = learningPlanId;
            CourseId = courseId;
            DateTimeRange = dateTimeRange;
        }

        public void FinishCourse()
        {
            var updatedDateTimeRange = new DateTimeRange(DateTimeRange.StartTime, DateTime.Now);
            DateTimeRange = updatedDateTimeRange;
        }
    }
}