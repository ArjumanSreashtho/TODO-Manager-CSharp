using AutoMapper;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.Task;

public class UpdateTaskProfile : Profile
{
    public UpdateTaskProfile()
    {
        CreateMap<UpdateTaskDTO, TaskModel>()
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
        );
    }
}