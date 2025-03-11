using AutoMapper;
using Domain.Models;
using Infrastructure.Entity;

namespace Infrastructure.Mapper
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentEntity, Student>()
            .ForMember(dest => dest.Email.address, opt => opt.MapFrom(src => src.Email));

        }
    }
}