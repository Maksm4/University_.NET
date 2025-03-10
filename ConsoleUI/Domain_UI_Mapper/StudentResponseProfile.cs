using AutoMapper;
using ConsoleUI.DTOs;
using Domain.Models;

namespace ConsoleUI.Domain_UI_Mapper
{
    public class StudentResponseProfile : Profile
    {
        public StudentResponseProfile()
        {
            CreateMap<Student, StudentResponse>();
        }
    }
}