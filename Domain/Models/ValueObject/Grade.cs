namespace Domain.Models.ValueObject
{
    public record Grade
    {
        public int Value { get; }

        private Grade() { }

        public Grade(int value)
        {
            if (value < 3 || value > 5)
            {
                throw new ArgumentException(nameof(value));
            }

            Value = value;
        }
    }
}