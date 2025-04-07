namespace ApplicationCore.DTO
{
    public class CourseModuleWithMarkDTO
    {
        public int? Mark { get; set; }
        public int CourseModuleId { get; set; }
        public required string Description { get; set; }
    }
}