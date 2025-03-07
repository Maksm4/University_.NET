using ApplicationCore.Context;
using ApplicationCore.IRepository;
using ApplicationCore.IService;
using Infrastructure.Repository;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleUI
{
    public static class Configuration
    {
        public static IHost IntitializeServices(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
            builder.Services.AddSingleton<ICourseRepository, CourseRepository>();

            builder.Services.AddSingleton<ICourseService, CourseService>();
            builder.Services.AddSingleton<IStudentService, StudentService>();
            builder.Services.AddSingleton<DbContext, UniversityContext>();

            return builder.Build();
        }
    }
}