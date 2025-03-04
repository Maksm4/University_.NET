using DataLayer.Context;
using DataLayer.IRepository;
using DataLayer.IService;
using Infrastructure.Repository;

namespace Infrastructure.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository StudentRepository;
        private readonly IUnitOfWork UnitOfWork;

        public StudentService(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;  
            StudentRepository = studentRepository;
        }

    }
}