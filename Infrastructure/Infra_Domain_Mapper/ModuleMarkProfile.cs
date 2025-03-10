using AutoMapper;
using Domain.Models;
using Infrastructure.Entity;

namespace Infrastructure.Mapper
{
    public class ModuleMarkProfile : Profile
    {
        public ModuleMarkProfile()
        {
            CreateMap<ModuleMarkEntity, ModuleMark>();
        }
    }
}