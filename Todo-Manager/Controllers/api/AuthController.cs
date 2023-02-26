using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Todo_Manager.Data;
using Todo_Manager.DTO.Authentication;
using Todo_Manager.Helper;
using Todo_Manager.Models;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Controllers.api;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var accessToken = await _authService.Login(loginDTO);
        return Ok(new
        {
            accessToken
        });
    }

    [Route("registration")]
    [HttpPost]
    public async Task<IActionResult> Registration([FromBody] RegistrationDTO registrationDto)
    {
        var result = await _authService.Registration(registrationDto);
        return Ok(new
        {
            accessToken = result.Value.Item1,
            data = result.Value.Item2
        });
    }
}