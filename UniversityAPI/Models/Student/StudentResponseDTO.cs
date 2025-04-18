using Newtonsoft.Json;

namespace UniversityAPI.Models.Student
{
    public class StudentResponseDTO
    {
        [JsonProperty("Id")]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}