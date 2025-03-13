using ApplicationCore.Context;
using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Service;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace UniversityAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UniversityContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("UniversityConnection"))
                );

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();

            builder.Services.AddScoped<ICourseReadService, CourseReadService>();
            builder.Services.AddScoped<IStudentReadService, StudentReadService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}