using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Controllers.api;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "USER,ADMIN")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMediator _mediator;
    public UserController(IUserService userService, IMediator mediator)
    {
        _userService = userService;
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDTO newUser)
    {
        var user = await _userService.CreateUser(newUser);
        return Ok(new
        {
            message = "User has been created",
            data = user
        });
    }

    [HttpGet]
    [Route("profile")]
    public async Task<IActionResult> GetUser()
    {
        var principal = HttpContext.User;
        var username = principal?.Claims.SingleOrDefault(claim => claim.Type.Contains("nameidentifier"))?.Value;
        var user = await _userService.GetUser(username);
        return Ok(new
        {
            message = "User has been retrieved",
            data = user
        });
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetUsers([FromQuery] bool? type = null, string search = "", int page = 1, int total = 10)
    {
        var totalUsers = await _userService.CountUsers(type, search);
        var userList = await _userService.GetUsers(type, search, page, total);
        return Ok(new
        {
            message = "Tasks have been retrieved",
            data = new
            {
                total = totalUsers,
                list = userList
            }
        });
    }

    [HttpGet]
    [Route("workable")]
    public async Task<IActionResult> GetWorkableUsers()
    {
        var userList = await _userService.GetWorkableUsers();
        return Ok(new
        {
            data = userList
        });
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserDTO updateUserDto)
    {
        var updatedUser = await _userService.UpdateUser(id, updateUserDto);
        return Ok(new
        {
            message = "User has been updated",
            data = updatedUser
        });
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var user = await _userService.DeleteUser(id);
        return Ok(new
        {
            message = "User has been deleted",
        });
    }

}