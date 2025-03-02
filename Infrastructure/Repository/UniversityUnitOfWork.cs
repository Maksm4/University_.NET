using dataLayer.Context;
using DataLayer.Context;
using DataLayer.IRepository;

namespace Infrastructure.Repository
{
    public class UniversityUnitOfWork : IUnitOfWork
    {
        public UniversityContext context { get; }
        public IStudentRepository studentRepository { get; }

        public IStudentRepository StudentRepository => studentRepository;

        public UniversityContext UniversityContext => context;

        public UniversityUnitOfWork(UniversityContext universityContext, IStudentRepository studentRepository)
        {
            this.context = universityContext;
            this.studentRepository = studentRepository;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}