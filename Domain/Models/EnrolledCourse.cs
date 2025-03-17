using Domain.Models.VObject;

namespace Domain.Models
{
    public class EnrolledCourse 
    {
        public int LearningPlanId { get; set; }
        public int CourseId { get; set; }
        public DateTimeRange DateTimeRange { get; private set; }
        private readonly IList<MarkedModule> _markedModules = new List<MarkedModule>();
        public IReadOnlyCollection<MarkedModule> MarkedModules => _markedModules.AsReadOnly();

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
            _markedModules.Add(markedModule);
        }
    }
}