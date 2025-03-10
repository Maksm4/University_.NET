using AutoMapper;
using ConsoleUI.DTOs;
using Domain.Models.ValueObject;

namespace ConsoleUI.Domain_UI_Mapper
{
    public class MarkResponseProfile : Profile
    {
        public MarkResponseProfile()
        {
            CreateMap<Grade, MarkResponse>()
                .ForMember(dest => dest.CourseModuleId, opt => opt.MapFrom(src => src.Module.CourseModuleId))
                .ForMember(dest => dest.Mark, opt => opt.MapFrom(src => src.Value));
        }
    }
}