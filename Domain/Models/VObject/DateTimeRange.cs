
using ApplicationCore.CustomExceptions;

namespace Domain.Models.VObject
{
    public class DateTimeRange
    {
        public DateOnly StartTime { get; init; }
        public DateOnly? EndTime { get; init; }

        public DateTimeRange(DateOnly startTime, DateOnly? endTime)
        {
            if (startTime > endTime && endTime != null)
            {
                throw new DateRangeException("start time cant be after end time");
            }
            StartTime = startTime;
            EndTime = endTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartTime, EndTime);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not DateTimeRange other)
            {
                return false;
            }
            return StartTime == other.StartTime && EndTime == other.EndTime;
        }
    }
}