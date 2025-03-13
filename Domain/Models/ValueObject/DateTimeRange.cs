namespace Domain.Models.ValueObject
{
    public class DateTimeRange
    {
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }

        public DateTimeRange(DateTime startTime, DateTime? endTime)
        {
            if (startTime > endTime && endTime != null)
            {
                throw new ArgumentException("start time cant be after end time");
            }
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}