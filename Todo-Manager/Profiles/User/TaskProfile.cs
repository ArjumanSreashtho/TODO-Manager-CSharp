using AutoMapper;
using Todo_Manager.DTO.Task;
using Todo_Manager.DTO.User;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.Task;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<UserTaskModel, TasksAssignedDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.TaskId)
            )
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Task.Title)
            )
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.Task.CreatedAt)
            )
            .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.Task.UpdatedAt)
            );
        CreateMap<TaskModel, TaskDTO>()
        .ForMember(
            dest => dest.Users,
            opt => opt.MapFrom(src => src.UserTasks)
        );
    }
}