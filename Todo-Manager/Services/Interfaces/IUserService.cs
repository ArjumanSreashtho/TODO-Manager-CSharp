using Todo_Manager.DTO.User;
using Todo_Manager.Models;

namespace Todo_Manager.Services.Interfaces;

public interface IUserService
{
    public Task<RetrievedUserDTO> CreateUser(CreateUserDTO user);

    public Task<int> CountUsers(bool? type, string search);
    public Task<UserTaskDTO> GetUser(string username);
    public Task<List<RetrievedUserDTO>> GetUsers(bool? type, string search, int page, int total);
    public Task<List<RetrievedUserDTO>> GetWorkableUsers();
    public Task<RetrievedUserDTO> UpdateUser(Guid id, UpdateUserDTO updateUserDto);
    public Task<RetrievedUserDTO> DeleteUser(Guid id);
}