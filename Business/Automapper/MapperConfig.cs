using AutoMapper;
using Data.Entities;
using Dto.DepartmentDtos;
using Dto.TaskDtos;
using Dto.UserDtos;

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

            CreateMap<ToDoTask, TaskDto>().ForMember(dest => dest.CreaterUser, opt => opt.MapFrom(src => src.CreaterUser.Name)).ReverseMap();
            CreateMap<TaskCreateDto, ToDoTask>().ReverseMap();
            CreateMap<TaskUpdateDto, ToDoTask>().ReverseMap();
        }
    }
}
