using ApplicationCore.DTO;
using AutoMapper;
using Domain.Models.Aggregate;
using UniversityAPI.Models.Student;

namespace UniversityAPI.Mapper
{
    public class StudentMapper : Profile
    {

        public StudentMapper()
        {
            CreateMap<Student, StudentResponseDTO>();

            CreateMap<StudentCourseTakenDTO, EnrolledCourseDTO>();
        }
    }
}
