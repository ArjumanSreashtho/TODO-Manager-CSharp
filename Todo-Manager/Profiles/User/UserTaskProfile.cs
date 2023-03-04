using AutoMapper;
using Todo_Manager.DTO.User;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.User;

public class UserTaskProfile : Profile
{
    public UserTaskProfile()
    {
        CreateMap<UserTaskModel, TasksAssignedDTO>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.User)
            )
            .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.User.Name)
            );
        CreateMap<UserModel, UserTaskDTO>()
            .ForMember(
                dest => dest.Tasks,
                opt => opt.MapFrom(src => src.UserTasks)
            );

    }
}