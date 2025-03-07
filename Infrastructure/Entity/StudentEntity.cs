namespace Infrastructure.Entity
{
    public class StudentEntity
    {
        public required int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public LearningPlanEntity LearningPlan { get; set; }
        public ICollection<ModuleMarkEntity> ModuleMarks { get; set; } = new List<ModuleMarkEntity>();
    }
}