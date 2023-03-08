using AutoMapper;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.Task;

public class TaskDetailsProfile : Profile
{
    public TaskDetailsProfile()
    {
        CreateMap<UserTaskModel, TaskUserDTO>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.User.Name)
            );
        CreateMap<TaskModel, TaskDTO>();
    }
}