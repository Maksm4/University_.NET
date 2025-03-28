using AutoMapper;
using Domain.Models;
using Domain.Models.Aggregate;
using Infrastructure.Context;
using WebApp.Models.ViewModel;

namespace WebApp.ViewModelMappers
{
    public class MapperViewModelProfile : Profile
    {
        public MapperViewModelProfile()
        {
            CreateMap<User, ProfileViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.student != null ? src.student.FirstName : ""))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.student != null ? src.student.LastName : ""))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.student != null ? src.student.BirthDate : DateOnly.MaxValue))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<Course, CourseViewModel>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => !src.Deprecated));

            CreateMap<Student, StudentInfoViewModel>();
            CreateMap<MarkedModule, MarkViewModel>();
        }
    }
}