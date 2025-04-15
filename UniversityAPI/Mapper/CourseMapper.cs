using AutoMapper;
using Domain.Models.Aggregate;
using UniversityAPI.Models;

namespace UniversityAPI.Mapper
{
    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            CreateMap<Course, CourseDTO>()
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.Deprecated));
            CreateMap<CourseForCreationDTO, Course>()
               .ForMember(dest => dest.IsDeprecated, opt => opt.MapFrom(src => !src.IsActive));
        }
    }
}