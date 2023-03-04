using AutoMapper;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.User;

public class CreateUserProfile : Profile
{
    public CreateUserProfile()
    {
        CreateMap<CreateUserDTO, UserModel>()
            .ForMember(
                dest => dest.Password,
                opt => opt.MapFrom(src => HashingPassword.HashPassword(src.Password))
            );
    }
}