using Domain.Models.Aggregate;
using Domain.Models.ValueObject;

namespace Domain.Models
{
    public class Student : BaseEntity, IAggregateRoot
    {
        public int StudentId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public Email Email { get; set; }
        public DateTime BirthDate { get; }
        public LearningPlan LearningPlan { get; set; }
        public ICollection<ModuleMark> ModuleMarks { get; set; } = new List<ModuleMark>();

        public Student(string FirstName, string lastName, Email email, DateTime birthDate, LearningPlan learningPlan)
        {
            this.FirstName = FirstName;
            this.LastName = lastName;
            this.Email = email;
            this.BirthDate = birthDate;
            this.LearningPlan = learningPlan;
        }

        public IReadOnlyList<IndividualCourse> GetCourses()
        {
            return LearningPlan.IndividualCourses.ToList();
        }

        public IEnumerable<Grade> GetGradesFromCourse(int courseId)
        {
            return ModuleMarks
                .Where(mm => mm.CourseModule.CourseId == courseId)
                .Select(mm => new Grade(mm.Mark, mm.CourseModule));
        }
    }
}