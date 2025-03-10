using AutoMapper;
using ConsoleUI.DTOs;
using Domain.Models;

namespace ConsoleUI.Domain_UI_Mapper
{
    public class CourseResponseProfile : Profile
    {
        public CourseResponseProfile()
        {
            CreateMap<Course, CourseResponse>();
        }
    }
}