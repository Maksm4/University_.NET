﻿using ApplicationCore.IRepository;
using Domain.Models.Aggregate;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class StudentRepository : CRUDRepository<Student>, IStudentRepository
    {
        private readonly UniversityContext dbContext;
        public StudentRepository(UniversityContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<Student>> GetAllStudentsWithEnrolledCoursesAsync()
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ThenInclude(lp => lp.EnrolledCourses)
                .ThenInclude(ec => ec.MarkedModules)
                .ToListAsync();
        }

        public override async Task<Student?> FindByIdAsync(int studentId)
        {
            return await dbContext.Students
                .Include(s => s.LearningPlan)
                .ThenInclude(lp => lp.EnrolledCourses)
                .ThenInclude(ec => ec.MarkedModules)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public bool IsDetached(Student student)
        {
            return dbContext.Entry(student).State == EntityState.Detached;
        }
    }
}