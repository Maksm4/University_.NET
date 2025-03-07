namespace Infrastructure.Entity
{
    public class IndividualCourseEntity
    {
        public int LearningPlanId { get; set; }
        public LearningPlanEntity LearningPlan { get; set; }
        public int CourseId { get; set; }
        public CourseEntity Course { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}