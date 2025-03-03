namespace DataLayer.Models
{
    public class LearningPlan
    {
        public int LearningPlanId { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public ICollection<IndividualCourse> IndividualCourses { get; set; } = new List<IndividualCourse>();
    }
}