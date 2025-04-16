namespace UniversityAPI.Models.Student
{
    public class StudentRequestDTO
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateOnly BirthDate { get; }
    }
}