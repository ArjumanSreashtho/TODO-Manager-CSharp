using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Todo_Manager.Data;
using Todo_Manager.DTO.Authentication;
using Todo_Manager.Models;

namespace Todo_Manager.Controllers.api;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private IConfiguration _config;
    private AppDbContext _appDbContext;
    public AuthController(IConfiguration config, AppDbContext appDbContext)
    {
        _config = config;
        _appDbContext = appDbContext;
    }

    
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await AuthenticateUser(loginDTO);
        
        if (user == null)
            return NotFound(new
            {
                success = false,
                message = "Invalid username or password"
            });
        var token = GenerateToken(user);

        return Ok(new
        {
            success = true,
            token
        });
    }

    async private Task<UserModel> AuthenticateUser(LoginDTO loginDto)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(
            user => user.Username == loginDto.Username && user.Password == loginDto.Password);
        return user;
    }

    private string GenerateToken(UserModel user)
    {
        var secrectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecrectKey"]));
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Role),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var accessToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
        //var refreshToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(24), signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
}