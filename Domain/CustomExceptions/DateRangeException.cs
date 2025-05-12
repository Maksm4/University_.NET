namespace ApplicationCore.CustomExceptions
{
    public class DateRangeException : Exception
    {
        public DateRangeException() : base() { }

        public DateRangeException(string message) : base(message) { }   
    }
}