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
            .ForMember(dest => dest.AsaignedUserLastName, opt => opt.MapFrom(src => src.AsaignedUser.LastName))
            .ReverseMap();

            CreateMap<TaskCreateDto, ToDoTask>().ReverseMap();
            CreateMap<TaskUpdateDto, TaskDtoWeb>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<DepartmentCreateDto, DepartmentDto>().ReverseMap();

            CreateMap<UserResult, UserViewModel>()
           .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
           .ForMember(dest => dest.AssignedTasks, opt => opt.MapFrom(src => src.AssignedTasks));

            CreateMap<UserResult, ProfileVM>().ReverseMap();
            CreateMap<UserDto, ProfileVM>().ReverseMap();

            CreateMap<ProfileUpdateVM, UserResult>().ReverseMap();
            CreateMap<UserDto, UserUpdateDto>().ReverseMap();
        }
    }
}
