using AutoMapper;
using OnlineOrderApi.Models;
using OnlineOrderApi.Dto;

namespace OnlineOrderApi
{
  public class MappingConfig : Profile
  {
    public MappingConfig()
    {
      //CreateMap<Student, StudentDTO>().ReverseMap();
      CreateMap<Student, StudentDTO>();
      CreateMap<StudentDTO, Student>();

      CreateMap<Student, StudentCreateDTO>().ReverseMap();
      CreateMap<Student, StudentUpdateDTO>().ReverseMap();
    }
  }
}