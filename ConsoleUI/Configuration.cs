﻿using ApplicationCore.IRepository;
using ApplicationCore.IService;
using ApplicationCore.Service;
using ConsoleUI.Domain_UI_Mapper;
using Infrastructure.Context;
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

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();


            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddAutoMapper(typeof(ResponseProfile));
            return builder.Build();
        }
    }
}