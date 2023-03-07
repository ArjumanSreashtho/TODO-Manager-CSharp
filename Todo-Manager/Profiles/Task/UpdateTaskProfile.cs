using AutoMapper;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.Task;

public class UpdateTaskProfile : Profile
{
    public UpdateTaskProfile()
    {
        CreateMap<UpdateTaskDTO, TaskModel>();
    }
}