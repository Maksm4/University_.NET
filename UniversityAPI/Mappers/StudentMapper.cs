using ApplicationCore.DTO;
using AutoMapper;
using Domain.Models;
using Domain.Models.Aggregate;
using Domain.Models.VObject;
using UniversityAPI.Models.EnrolledCourse;
using UniversityAPI.Models.Student;

namespace UniversityAPI.Mappers
{
    public class StudentMapper : Profile
    {

        public StudentMapper()
        {
            CreateMap<Student, StudentResponseDTO>();

            CreateMap<StudentRequestDTO, Student>();

            CreateMap<Student, StudentRequestDTO>();
            CreateMap<StudentCourseTakenDTO, EnrolledCourseResponseDTO>()
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.DateTimeRange.StartTime))
               .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.DateTimeRange.EndTime))
               .ForMember(dest => dest.markedCourseModules, opt => opt.MapFrom(src => src.courseModuleWithMarkDTOs));


            CreateMap<EnrolledCourseRequestDTO, EnrolledCourse>()
               .ForMember(dest => dest.DateTimeRange, opt => opt.MapFrom(src => new DateTimeRange(src.StartDate ?? DateOnly.FromDateTime(DateTime.Now), src.EndDate)));
        }
    }
}
