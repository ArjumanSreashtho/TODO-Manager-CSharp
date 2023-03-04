using AutoMapper;
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
    private readonly IMapper _mapper;
    public UserService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<RetrievedUserDTO> CreateUser(CreateUserDTO user)
    {
        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(exUser => exUser.Username == user.Username);
        if (existingUser != null)
            throw new CustomException("Username already exists", 400);
        var newUser = _mapper.Map<UserModel>(user);

        await _appDbContext.Users.AddAsync(newUser);
        await _appDbContext.SaveChangesAsync();
        var userResponse = _mapper.Map<RetrievedUserDTO>(newUser);
        return userResponse;
    }

    public async Task<int> CountUsers(bool? type = null, string search = "")
    {
        var totalUser = await _appDbContext.Users.Where(user => user.Name.Contains(search)).CountAsync();
        return totalUser;
    }

    public async Task<UserTaskDTO> GetUser(string username)
    {
        var user = await _appDbContext.Users
            .Where(user => user.Username == username)
            .Include(user => user.UserTasks)
            .ThenInclude(userTask => userTask.Task)
            .FirstOrDefaultAsync();
        if (user == null)
            throw new CustomException("Not found", 404);
        var response = _mapper.Map<UserTaskDTO>(user);
        return response;
    }

    public async Task<List<RetrievedUserDTO>> GetUsers(bool? type = null, string search = "", int page = 1, int total = 10)
    {
        var userList = await _appDbContext.Users.Where(user => user.Name.Contains(search)).OrderByDescending(user => user.UpdatedAt).Skip((page - 1) * total).Take(total).Select(user => _mapper.Map<RetrievedUserDTO>(user)).ToListAsync();
        return userList;
    }

    public async Task<List<RetrievedUserDTO>> GetWorkableUsers()
    {
        var userList = await _appDbContext.Users.Select(user => _mapper.Map<RetrievedUserDTO>(user)).ToListAsync();
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

        var userResponse = _mapper.Map<RetrievedUserDTO>(user);
        return userResponse;
    }

    public async Task<RetrievedUserDTO> DeleteUser(Guid id)
    {
        var user = await _appDbContext.Users.FindAsync(id);
        if (user == null)
            throw new CustomException("Not found", 404);
        _appDbContext.Remove(user);
        await _appDbContext.SaveChangesAsync();
        var userResponse = _mapper.Map<RetrievedUserDTO>(user);
        return userResponse;
    }
}