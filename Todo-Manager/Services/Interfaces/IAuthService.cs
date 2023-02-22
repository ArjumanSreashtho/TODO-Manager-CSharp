using Todo_Manager.DTO.Authentication;
using Todo_Manager.Models;

namespace Todo_Manager.Services.Interfaces;

public interface IAuthService
{
    public Task<string?> Login(LoginDTO loginDTO);
    public Task<(string, UserModel)?> Registration(RegistrationDTO registrationDto);
}