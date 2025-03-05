using ApplicationCore.Context;

namespace Infrastructure.Repository
{
    public class UniversityUnitOfWork : IUnitOfWork
    {
        private readonly UniversityContext context;

        public UniversityUnitOfWork(UniversityContext universityContext)
        {
            this.context = universityContext;
        }

        public async Task SaveAsync()
        {
            //could add here some interceptors
            await context.SaveChangesAsync();
        }
    }
}