using AutoMapper;
using Todo_Manager.DTO.User;
using Todo_Manager.Models;

namespace Todo_Manager.Profiles.User;

public class RetriveUserProfile : Profile
{
    public RetriveUserProfile()
    {
        CreateMap<UserModel, RetrievedUserDTO>();
    }
}