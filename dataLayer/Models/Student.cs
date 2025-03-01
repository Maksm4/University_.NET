namespace dataLayer.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public LearningPlan LearningPlan { get; set; }
        public IList<ModuleMark> moduleMarks { get; set; }
    }
}