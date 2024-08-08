using AutoMapper;
using Data.DepartmentDtos;
using Data.Entities;
using Data.UserDtos;

namespace Business.Automapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Department, DepartmentCreateDto>().ReverseMap();
            CreateMap<Department, DepartmentUpdateDto>().ReverseMap();

            CreateMap<ToDoTask, TaskDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.AsaignedUserName, opt => opt.MapFrom(src => src.AsaignedUser.Name))
                .ForMember(dest => dest.AsaignedUserEmail, opt => opt.MapFrom(src => src.AsaignedUser.Email)) // Atanan kişinin e-posta adresi
                .ForMember(dest => dest.CreaterUser, opt => opt.MapFrom(src => src.CreaterUser.Name))
                .ForMember(dest => dest.CreaterUserEmail, opt => opt.MapFrom(src => src.CreaterUser.Email)) // Atayan kişinin e-posta adresi
                .ReverseMap();
        }
    }
}
