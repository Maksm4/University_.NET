using ApplicationCore.Context;
using ApplicationCore.IRepository;
using ApplicationCore.IService;

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