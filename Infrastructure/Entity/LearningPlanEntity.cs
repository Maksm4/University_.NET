namespace Infrastructure.Entity
{
    public class LearningPlanEntity
    {
        public int LearningPlanId { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        public StudentEntity Student { get; set; } = null!;
        public ICollection<IndividualCourseEntity> IndividualCourses { get; set; } = new List<IndividualCourseEntity>();
    }
}