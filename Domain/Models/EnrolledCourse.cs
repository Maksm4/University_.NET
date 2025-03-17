using Domain.Models.VObject;

namespace Domain.Models
{
    public class EnrolledCourse : BaseEntity
    {
        public int LearningPlanId { get; set; }
        public int CourseId { get; set; }
        public DateTimeRange DateTimeRange { get; private set; }
        private readonly IList<MarkedModule> _moduleMarks = new List<MarkedModule>();
        public IEnumerable<MarkedModule> MarkedModules => _moduleMarks.AsReadOnly();

        private EnrolledCourse() { }
        internal EnrolledCourse(int learningPlanId, int courseId, DateTimeRange dateTimeRange)
        {
            LearningPlanId = learningPlanId;
            CourseId = courseId;
            DateTimeRange = dateTimeRange;
        }

        public void FinishCourse()
        {
            var updatedDateTimeRange = new DateTimeRange(DateTimeRange.StartTime, DateOnly.FromDateTime(DateTime.Now));
            DateTimeRange = updatedDateTimeRange;
        }

        public void AddMarkedModule(MarkedModule markedModule)
        {
            _moduleMarks.Add(markedModule);
        }
    }
}