using AutoMapper;
using Domain.Models;
using Infrastructure.Entity;

namespace Infrastructure.Mapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseEntity, Course>();
        }
    }
}