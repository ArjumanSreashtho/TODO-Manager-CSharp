using AutoMapper;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.Task;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskModel, TaskDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title)
            )
            .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description)
            )
            .ForMember(
                dest => dest.Completed,
                opt => opt.MapFrom(src => src.Completed)
            )
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt)
            )
            .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.MapFrom(src => src.UpdatedAt)
            )
            .ForMember(
                dest => dest.Users,
                opt => opt.MapFrom(src => src.UserTasks.Select(userTask => new
                    TaskUserDTO()
                    {
                        Id = userTask.User.Id,
                        Name = userTask.User.Name,

                    }).ToList())
            );
    }
}