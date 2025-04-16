namespace UniversityAPI.Models.Student
{
    public class StudentForCreationDTO
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateOnly BirthDate { get; }
    }
}