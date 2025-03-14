using Domain.Models.VObject;

namespace Domain.Models.Aggregate
{
    public class Student : BaseEntity, IAggregateRoot
    {
        public int StudentId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public Email Email { get; set; }
        public DateTime BirthDate { get; }
        public LearningPlan LearningPlan { get; private set; }

        private Student() { }
        public Student(int studentId, string firstName, string lastName, Email email, DateTime birthDate, LearningPlan learning)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            LearningPlan = learning;
        }
        
        public IEnumerable<EnrolledCourse> GetEnrolledCourses()
        {
            return LearningPlan.EnrolledCourses;
        }

        public void EnrollInCourse(Course course)
        {
            var enrolledCourse = new EnrolledCourse(LearningPlan.LearningPlanId, course.CourseId, new DateTimeRange(DateOnly.FromDateTime(DateTime.Now), null));
            LearningPlan.AddCourse(enrolledCourse);
        }

        public void FinishCourse(Course course)
        {
            var enrolledCourse = LearningPlan.GetEnrolledCourse(course);
            if (enrolledCourse != null)
            {
                enrolledCourse.FinishCourse();
            }
        }

        public List<MarkedModule> GetMarksFromCourse(Course course)
        {
           return LearningPlan.EnrolledCourses.SelectMany(ec => ec.MarkedModules).ToList();
        }
    }
}