using AutoMapper;
using Domain.Models;
using Domain.Models.Aggregate;
using UniversityAPI.Models;

namespace UniversityAPI.Mapper
{
    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            CreateMap<Course, CourseResponseDTO>()
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.Deprecated))
               .ForMember(dest => dest.courseModuleDTOs, opt => opt.MapFrom(src => src.CourseModules));
            CreateMap<CourseForCreationDTO, Course>()
               .ForMember(dest => dest.IsDeprecated, opt => opt.MapFrom(src => !src.IsActive));

            CreateMap<Course, CourseForUpdateDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.Deprecated));
            CreateMap<CourseForUpdateDTO, Course>()
                .ForMember(dest => dest.IsDeprecated, opt => opt.MapFrom(src => !src.IsActive));

            CreateMap<CourseModule, CourseModuleResponseDTO>();
        }
    }
}