using AutoMapper;
using ConsoleUI.DTOs;
using Domain.Models;
using Domain.Models.Aggregate;

namespace ConsoleUI.Domain_UI_Mapper
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap<Course, CourseResponse>();
            CreateMap<MarkedModule, MarkResponse>()
                .ForMember(dest => dest.CourseModuleId, opt => opt.MapFrom(src => src.CourseModuleId))
                .ForMember(dest => dest.Mark, opt => opt.MapFrom(src => src.Mark));
            CreateMap<Student, StudentResponse>();
        }
    }
}