using DataLayer.GenericRepositories;
using DataLayer.IRepository;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class StudentRepository : CRUDRepository<Student>, IStudentRepository
    {
        private readonly DbContext dbContext;
        public StudentRepository(DbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }



    }
}