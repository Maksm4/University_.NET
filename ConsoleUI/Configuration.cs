using ApplicationCore.Context;
using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Service;
using Infrastructure.Mapper;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleUI
{
    public static class Configuration
    {
        public static IHost IntitializeServices(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddDbContext<UniversityContext>(
               opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("UniversityConnection"))
               );

            builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
            builder.Services.AddSingleton<ICourseRepository, CourseRepository>();


            builder.Services.AddSingleton<ICourseService, CourseService>();
            builder.Services.AddSingleton<IStudentService, StudentService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return builder.Build();
        }
    }
}