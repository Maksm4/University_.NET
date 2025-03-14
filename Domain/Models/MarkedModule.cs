namespace Domain.Models
{
    public class MarkedModule : BaseEntity
    {
        public int CourseId { get; set; }
        public int CourseModuleId { get; set; }
        public int LearningPlanId { get; set; }
        public int Mark { get; private set; }
        private MarkedModule() { }

        public MarkedModule(int learningPlanId, int courseId, int courseModuleId, int mark)
        {
            LearningPlanId = learningPlanId;
            CourseId = courseId;
            CourseModuleId = courseModuleId;
            if (mark < 3 || mark > 5)
            {
                throw new ArgumentException("mark has to be in 3-5 range");
            }
            Mark = mark;
        }
    }
}