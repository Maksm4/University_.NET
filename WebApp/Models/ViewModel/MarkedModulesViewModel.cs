namespace WebApp.Models.ViewModel
{
    public class MarkedModulesViewModel
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public IReadOnlyCollection<MarkViewModel> markViewModels { get; set; }

    }
}