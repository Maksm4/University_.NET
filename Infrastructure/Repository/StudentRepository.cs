using dataLayer.Models;
using DataLayer.GenericRepositories;
using DataLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class StudentRepository : CRUDRepository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext dbContext) : base(dbContext)
        {
        }



    }
}