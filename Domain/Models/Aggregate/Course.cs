﻿namespace Domain.Models.Aggregate
{
    public class Course : IAggregateRoot
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deprecated { get; set; }
        private readonly List<CourseModule> _courseModules = new List<CourseModule>();
        public IReadOnlyCollection<CourseModule> CourseModules => _courseModules.AsReadOnly();

        public bool IsDeprecated => Deprecated;

        private Course() { }

        public Course(int courseId, string name, string description, bool deprecated)
        {
            CourseId = courseId;
            Name = name;
            Description = description;
            Deprecated = deprecated;
        }

        public void AddModule(string description)
        {
            var module = new CourseModule(CourseId, description);
            _courseModules.Add(module);
        }
    }
}