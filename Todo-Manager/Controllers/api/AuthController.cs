using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Todo_Manager.Data;
using Todo_Manager.Domain.Auth.Queries;
using Todo_Manager.DTO.Authentication;
using Todo_Manager.Helper;
using Todo_Manager.Models;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Controllers.api;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IAuthService authService, IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var accessToken = await _mediator.Send(new GetAuthTokenQuery()
        {
            LoginDTO = loginDTO
        });
        return Ok(new
        {
            accessToken
        });
    }
}