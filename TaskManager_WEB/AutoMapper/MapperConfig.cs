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

            CreateMap<ToDoTask, TaskDtoWeb>().ReverseMap();

            CreateMap<TaskCreateDto, ToDoTask>().ReverseMap();
            CreateMap<TaskUpdateDto, TaskDtoWeb>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<DepartmentCreateDto, DepartmentDto>().ReverseMap();

            CreateMap<UserResult, UserViewModel>().ReverseMap();
            CreateMap<UserResult, ProfileVM>().ReverseMap();
            CreateMap<UserDto, ProfileVM>().ReverseMap();

            CreateMap<ProfileUpdateVM, UserResult>().ReverseMap();
            CreateMap<UserDto, UserUpdateDto>().ReverseMap();
        }
    }
}
