namespace Domain.Models.ValueObject
{
    public class DateTimeRange
    {
        public DateOnly StartTime { get; init; }
        public DateOnly? EndTime { get; init; }

        public DateTimeRange(DateOnly startTime, DateOnly? endTime)
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