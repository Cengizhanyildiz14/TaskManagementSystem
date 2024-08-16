using AutoMapper;
using Data.Entities;
using TaskManager_WEB.Models;

namespace TaskManager_WEB.AutoMapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<ToDoTask, TaskDtoWeb>()
            .ForMember(dest => dest.AsaignedUserName, opt => opt.MapFrom(src => src.AsaignedUser.Name))
            .ForMember(dest => dest.AsaignedUserEmail, opt => opt.MapFrom(src => src.AsaignedUser.Email))
            .ReverseMap();

            CreateMap<TaskCreateDto, ToDoTask>().ReverseMap();
            CreateMap<TaskUpdateDto, TaskDtoWeb>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<DepartmentCreateDto, DepartmentDto>().ReverseMap();


        }
    }
}
