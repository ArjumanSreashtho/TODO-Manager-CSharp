using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles;

public class CreateUserProfile : Profile
{
    public CreateUserProfile()
    {
        CreateMap<CreateUserDTO, UserModel>()
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
                dest => dest.Password,
                opt => opt.MapFrom(src => HashingPassword.HashPassword(src.Password))
            );

    }
}