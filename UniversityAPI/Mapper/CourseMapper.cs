using ApplicationCore.DTO;
using AutoMapper;
using Domain.Models;
using Domain.Models.Aggregate;
using UniversityAPI.Models.Course;
using UniversityAPI.Models.EnrolledCourse;

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

            CreateMap<Course, CourseRequestDTO>();

            CreateMap<CourseModule, CourseModuleResponseDTO>();

            CreateMap<CourseModuleWithMarkDTO, MarkedCourseModuleResponseDTO>();
        }
    }
}