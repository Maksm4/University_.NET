using Domain.Models.VObject;

namespace Domain.Models.Aggregate
{
    public class Student : IAggregateRoot
    {
        public int StudentId { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public LearningPlan LearningPlan { get; private set; }

        private Student() { }
        public Student(string firstName, string lastName, DateOnly birthDate, LearningPlan learningPlan)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            LearningPlan = learningPlan;
        }

        public IReadOnlyCollection<EnrolledCourse> GetEnrolledCourses()
        {
            return LearningPlan.EnrolledCourses.ToList();
        }

        public void EnrollInCourse(Course course)
        {
            var enrolledCourse = new EnrolledCourse(LearningPlan.LearningPlanId, course.CourseId, new DateTimeRange(DateOnly.FromDateTime(DateTime.Now), null));
            LearningPlan.AddCourse(enrolledCourse);
        }
        public void EnrollInCourse(Course course, DateTimeRange dateRange)
        {
            var enrolledCourse = new EnrolledCourse(LearningPlan.LearningPlanId, course.CourseId, dateRange);
            LearningPlan.AddCourse(enrolledCourse);
        }

        public EnrolledCourse? GetEnrolledCourse(int courseId)
        {
            return LearningPlan.GetEnrolledCourse(courseId);
        }

        public void WithdrawFromCourse(int courseId)
        {
            var enrolledCourse = LearningPlan.GetEnrolledCourse(courseId);
            if (enrolledCourse != null)
            {
                LearningPlan.RemoveCourse(enrolledCourse);
            }
        }

        public void FinishCourse(Course course)
        {
            var enrolledCourse = LearningPlan.GetEnrolledCourse(course.CourseId);
            if (enrolledCourse != null)
            {
                enrolledCourse.FinishCourse();
            }
        }

        public IReadOnlyCollection<MarkedModule> GetMarksFromCourse(int courseId)
        {
            return LearningPlan.EnrolledCourses.Where(ec => ec.CourseId == courseId).SelectMany(ec => ec.MarkedModules).ToList();
        }

        public bool GiveMark(CourseModule courseModule, int grade)
        {
            var markedModule = new MarkedModule(LearningPlan.LearningPlanId, courseModule.CourseId, courseModule.CourseModuleId, grade);
            var enrolledCourse = LearningPlan.GetEnrolledCourse(courseModule.CourseId);

            if (enrolledCourse != null)
            {
                enrolledCourse.AddMarkedModule(markedModule);
                return true;
            }
            return false;
        }

        public bool HasPassed(Course course)
        {

            var enrolledCourse = LearningPlan.GetEnrolledCourse(course.CourseId);
            if (enrolledCourse != null)
            {
                if (enrolledCourse.MarkedModules.Count() == course.CourseModules.Count())
                {
                    return true;
                }
            }
            return false;
        }
    }
}