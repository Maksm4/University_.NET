using AutoMapper;
using Domain.Models;
using Infrastructure.Entity;

namespace Infrastructure.Mapper
{
    public class IndividualCourseProfile : Profile
    {
        public IndividualCourseProfile()
        {
            CreateMap<IndividualCourseEntity, IndividualCourse>();
        }
    }
}