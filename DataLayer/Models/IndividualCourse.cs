namespace DataLayer.Models
{
    public class IndividualCourse
    {
        public int LearningPlanId { get; set; }
        public LearningPlan LearningPlan { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}