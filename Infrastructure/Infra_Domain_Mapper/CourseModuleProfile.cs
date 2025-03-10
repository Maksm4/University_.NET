using AutoMapper;
using Domain.Models;
using Infrastructure.Entity;

namespace Infrastructure.Infra_Domain_Mapper
{
    public class CourseModuleProfile : Profile
    {
        public CourseModuleProfile()
        {
            CreateMap<CourseModuleEntity, CourseModule>();
        }
    }
}