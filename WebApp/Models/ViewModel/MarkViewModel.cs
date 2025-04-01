namespace WebApp.Models.ViewModel
{
    public class MarkViewModel
    {
        public int? Mark { get; set; }
        public int CourseModuleId { get; set; }
        public required string CourseDescription { get; set; }
    }
}