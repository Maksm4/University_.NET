﻿using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IRepository
{
    public interface IStudentRepository : ICRUDRepository<Student>
    {
        Task<IReadOnlyCollection<Student>> GetAllStudentsWithEnrolledCoursesAsync();
        bool IsDetached(Student student);
    }
}