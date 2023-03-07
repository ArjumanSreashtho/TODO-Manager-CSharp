using AutoMapper;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.Task;

public class TaskDetailsProfile : Profile
{
    public TaskDetailsProfile()
    {
        CreateMap<TaskModel, TaskDTO>()
            .ForMember(
                dest => dest.Users,
                opt => opt.MapFrom(src => src.UserTasks.Select(userTask => new TaskUserDTO()
                {
                    Id = userTask.User.Id,
                    Name = userTask.User.Name,
                }).ToList())
            );
    }
}