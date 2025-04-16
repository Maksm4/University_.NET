using AutoMapper;
using Domain.Models;
using Domain.Models.Aggregate;
using UniversityAPI.Models.Course;

namespace UniversityAPI.Mapper
{
    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            CreateMap<Course, CourseResponseDTO>()
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.Deprecated))
               .ForMember(dest => dest.CourseModuleDTOs, opt => opt.MapFrom(src => src.CourseModules));
            CreateMap<CourseRequestDTO, Course>()
               .ForMember(dest => dest.IsDeprecated, opt => opt.MapFrom(src => false));

            CreateMap<CourseModule, CourseModuleResponseDTO>();
        }
    }
}