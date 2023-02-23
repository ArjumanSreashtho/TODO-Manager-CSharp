using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Models;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _appDbContext;
    private readonly HashingPassword _hashingPassword;
    public UserService(AppDbContext appDbContext, HashingPassword hashingPassword)
    {
        _appDbContext = appDbContext;
        _hashingPassword = hashingPassword;
    }
    public async Task<RetrievedUserDTO> CreateUser(CreateUserDTO user)
    {
        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(exUser => exUser.Username == user.Username);
        if (existingUser != null)
            throw new CustomException("Username already exists", 400);
        var hashedPassword = _hashingPassword.HashPassword(user.Password);
        var newUser = new UserModel()
        {
            Username = user.Username,
            Name = user.Name,
            Password = hashedPassword,
            Role = user.Role
        };

        await _appDbContext.Users.AddAsync(newUser);
        await _appDbContext.SaveChangesAsync();
        var userResponse = new RetrievedUserDTO()
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Username = newUser.Username,
            Role = newUser.Role,
            CreatedAt = newUser.CreatedAt,
            UpdatedAt = newUser.UpdatedAt

        };
        return userResponse;
    }

    public async Task<int> CountUsers(bool? type = null, string search = "")
    {
        var totalUser = await _appDbContext.Users.Where(user => user.Name.Contains(search)).CountAsync();
        return totalUser;
    }

    public async Task<RetrievedUserDTO> GetUser(Guid id)
    {
        var user = await _appDbContext.Users.FindAsync(id);
        if (user == null)
            throw new CustomException("Not found", 404);
        var userResponse = new RetrievedUserDTO()
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
        return userResponse;
    }

    public async Task<List<RetrievedUserDTO>> GetUsers(bool? type = null, string search = "", int page = 1, int total = 10)
    {
        var userList = await _appDbContext.Users.Where(user => user.Name.Contains(search)).OrderByDescending(user => user.UpdatedAt).Skip((page - 1) * total).Take(total).Select(user => new RetrievedUserDTO()
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
            
        }).ToListAsync();
        return userList;
    }

    public async Task<RetrievedUserDTO> UpdateUser(Guid id, UpdateUserDTO updateUserDto)
    {
        var user = await _appDbContext.Users.FindAsync(id);

        if (user == null)
            throw new CustomException("Not found", 404);
        
        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Username == updateUserDto.Username);
        
        if (existingUser != null && user.Username != updateUserDto.Username)
            throw new CustomException("Username already exists", 400);
        
        user.Name = updateUserDto.Name != null ? updateUserDto.Name : user.Name;
        user.Username = updateUserDto.Username != null ? updateUserDto.Username : user.Username;

        var userResponse = new RetrievedUserDTO()
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
        return userResponse;
    }

    public async Task<RetrievedUserDTO> DeleteUser(Guid id)
    {
        var existingUser = await _appDbContext.Users.FindAsync(id);
        if (existingUser == null)
            throw new CustomException("Not found", 404);
        _appDbContext.Remove(existingUser);
        await _appDbContext.SaveChangesAsync();
        var userResponse = new RetrievedUserDTO()
        {
            Name = existingUser.Name,
            Username = existingUser.Username
        };
        return userResponse;
    }
}