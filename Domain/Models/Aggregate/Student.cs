using Domain.Models.VObject;
using System.Collections.ObjectModel;

namespace Domain.Models.Aggregate
{
    public class Student : IAggregateRoot
    {
        public int StudentId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateOnly BirthDate { get; }
        public LearningPlan? LearningPlan { get; private set; }

        private Student() { }
        public Student(string firstName, string lastName, DateOnly birthDate, LearningPlan learningPlan)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            LearningPlan = learningPlan;
        }

        public Student(string firstName, string lastName, DateOnly birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }
        public void AssignLearningPlan(LearningPlan learningPlan)
        {
            LearningPlan = learningPlan;
        }
        public IReadOnlyCollection<EnrolledCourse> GetEnrolledCourses()
        {
            if (LearningPlan != null)
            {
                return LearningPlan.EnrolledCourses.ToList();
            }
            return ReadOnlyCollection<EnrolledCourse>.Empty;
        }

        public void EnrollInCourse(Course course)
        {
            if (LearningPlan != null)
            {
                var enrolledCourse = new EnrolledCourse(LearningPlan.LearningPlanId, course.CourseId, new DateTimeRange(DateOnly.FromDateTime(DateTime.Now), null));
                LearningPlan.AddCourse(enrolledCourse);
            }
        }

        public void FinishCourse(Course course)
        {
            if (LearningPlan != null)
            {
                var enrolledCourse = LearningPlan.GetEnrolledCourse(course.CourseId);
                if (enrolledCourse != null)
                {
                    enrolledCourse.FinishCourse();
                }
            }
        }

        public IReadOnlyCollection<MarkedModule> GetMarksFromCourse(Course course)
        {
           return LearningPlan.EnrolledCourses.SelectMany(ec => ec.MarkedModules).ToList();
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