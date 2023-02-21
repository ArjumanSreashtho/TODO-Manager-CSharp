using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Controllers.api;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "USER,ADMIN")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly Hashing _hashing;
    public UserController(AppDbContext appDbContext, Hashing hashing)
    {
        _appDbContext = appDbContext;
        _hashing = hashing;
    }
    
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateUser(CreateUserDTO newUser)
    {
        var hashedPassword = _hashing.HashPassword(newUser.Password);
        var user = new UserModel()
        {
            Username = newUser.Username,
            Name = newUser.Name,
            Password = hashedPassword,
            Role = newUser.Role
        };

        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] bool? type = null, string search = "", int page = 1, int total = 10)
    {
        var totalData = await _appDbContext.Users.Where(user => user.Name.Contains(search)).CountAsync();
        var dataList = await _appDbContext.Users.Where(user => user.Name.Contains(search))
            .OrderByDescending(user => user.UpdatedAt).Skip((page - 1) * total).Take(total).ToListAsync();
        return Ok(new
        {
            totalData,
            dataList
        });
    }
}