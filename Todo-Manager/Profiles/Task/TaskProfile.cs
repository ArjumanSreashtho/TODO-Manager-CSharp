using AutoMapper;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.Task;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<UserTaskModel, TaskUserDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.User.Id)
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.User.Name)
            );
        CreateMap<TaskModel, TaskDTO>()
        .ForMember(
            dest => dest.Users,
            opt => opt.MapFrom(src => src.UserTasks)
        );
    }
}