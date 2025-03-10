namespace Domain.Models.ValueObject
{
    public record Grade
    {
        public int Value { get; }
        public CourseModule Module { get; }

        public Grade(int value, CourseModule courseModule)
        {
            if (value < 3 || value > 5)
            {
                throw new ArgumentException(nameof(value));
            }

            if (courseModule == null)
            {
                throw new ArgumentNullException(nameof(courseModule));
            }

            Value = value;
            Module = courseModule;
        }
    }
}