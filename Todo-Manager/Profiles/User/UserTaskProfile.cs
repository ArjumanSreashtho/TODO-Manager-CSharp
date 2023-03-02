using AutoMapper;
using Todo_Manager.DTO.User;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles;

public class UserTaskProfile : Profile
{
    public UserTaskProfile()
    {
        CreateMap<UserModel, UserTaskDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.Username)
            )
            .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => src.Role)
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
                dest => dest.Tasks,
                opt => opt.MapFrom(src => src.UserTasks.Select(userTask => new TasksAssignedDTO()
                {
                    Id = userTask.TaskId,
                    Title = userTask.Task.Title,
                    CreatedAt = userTask.Task.CreatedAt,
                    UpdatedAt = userTask.Task.UpdatedAt

                }).ToList())
            );
    }
}