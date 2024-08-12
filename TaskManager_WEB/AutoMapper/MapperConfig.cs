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

            // Task Mappings
            CreateMap<ToDoTask, TaskDto>()
            .ForMember(dest => dest.AsaignedUserName, opt => opt.MapFrom(src => src.AsaignedUser.Name))
            .ForMember(dest => dest.AsaignedUserEmail, opt => opt.MapFrom(src => src.AsaignedUser.Email))
            .ReverseMap();

            // Department Mappings
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<TaskCreateDto, ToDoTask>().ReverseMap();
            CreateMap<TaskUpdateDto, TaskDto>().ReverseMap();
        }
    }
}
