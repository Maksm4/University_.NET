namespace DataLayer.Models
{
    public class Student
    {
        public required int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public LearningPlan LearningPlan { get; set; }
        public ICollection<ModuleMark> ModuleMarks { get; set; } = new List<ModuleMark>();
    }
}