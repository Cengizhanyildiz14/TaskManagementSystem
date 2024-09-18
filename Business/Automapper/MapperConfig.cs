using AutoMapper;
using Data.Entities;
using Dto.AnnouncementDtos;
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

            CreateMap<Announcement, AnnouncementDto>().ReverseMap();
            CreateMap<Announcement, AnnouncementCreateDto>().ReverseMap();
            CreateMap<Announcement, AnnouncementUpdateDto>().ReverseMap();

            CreateMap<ToDoTask, TaskDto>().ForMember(dest => dest.CreaterUserName, opt => opt.MapFrom(src => src.CreaterUser.Name)).
                ForMember(dest => dest.CreaterUserLastName, opt => opt.MapFrom(src => src.CreaterUser.LastName)).ReverseMap();

            CreateMap<TaskCreateDto, ToDoTask>().ReverseMap();
            CreateMap<TaskUpdateDto, ToDoTask>().ReverseMap();
        }
    }
}
