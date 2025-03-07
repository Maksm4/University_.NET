using Domain.Models;

namespace ConsoleUI.DTOs
{
    public class CourseResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CourseModule> CourseModules { get; } = new List<CourseModule>();
    }
}