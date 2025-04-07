namespace ApplicationCore.DTO
{
    public class CourseWithEnrollmentStatusDTO
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnrolled { get; set; }
    }
}