using AutoMapper;
using Domain.Models;
using Infrastructure.Entity;

namespace Infrastructure.Mapper
{
    public class LearningPlanProfile : Profile
    {
        public LearningPlanProfile()
        {
            CreateMap<LearningPlanEntity, LearningPlan>();
        }
    }
}