using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo_Manager.Data;
using Todo_Manager.Domain.Users.Commands;
using Todo_Manager.Domain.Users.Queries;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Controllers.api;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "USER,ADMIN")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDTO user)
    {
        var newUser = await _mediator.Send(new CreateUserCommand()
        {
            User = user
        });
        return Ok(new
        {
            message = "User has been created",
            data = newUser
        });
    }

    [HttpGet]
    [Route("profile")]
    public async Task<IActionResult> GetUser()
    {
        var principal = HttpContext.User;
        var username = principal?.Claims.SingleOrDefault(claim => claim.Type.Contains("nameidentifier"))?.Value;
        var user = await _mediator.Send(new GetUserQuery()
        {
            Username = username
        });
        return Ok(new
        {
            message = "User has been retrieved",
            data = user
        });
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetUsers([FromQuery] string search = "", int page = 1, int total = 10)
    {
        var totalUsers = await _mediator.Send(new GetCountAllUsersQuery()
        {
            Search = search
        });
        
        var userList = await _mediator.Send(new GetAllUsersQuery()
        {
            Search = search,
            Page = page,
            Total = total
        });
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
        var userList = await _mediator.Send(new GetAllWorkableUsersQuery());
        return Ok(new
        {
            data = userList
        });
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserDTO updateUserDto)
    {
        var updatedUser = await _mediator.Send(new UpdateUserCommand()
        {
            Id = id,
            UpdateUserDto = updateUserDto
        });
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
        await _mediator.Send(new DeleteUserCommand()
        {
            Id = id
        });
        return Ok(new
        {
            message = "User has been deleted",
        });
    }

}