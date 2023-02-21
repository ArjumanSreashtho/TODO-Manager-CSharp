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

namespace Todo_Manager.Controllers.api;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private IConfiguration _config;
    private AppDbContext _appDbContext;
    private Hashing _hashing;
    public AuthController(IConfiguration config, AppDbContext appDbContext, Hashing hashing)
    {
        _config = config;
        _appDbContext = appDbContext;
        _hashing = hashing;
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
        var accessToken = GenerateToken(user);

        return Ok(new
        {
            success = true,
            message = "User has been logged successfully",
            accessToken
        });
    }

    [Route("registration")]
    [HttpPost]
    public async Task<IActionResult> Registration([FromBody] RegistrationDTO registrationDto)
    {
        if (registrationDto.Password != registrationDto.ConfirmPassword)
            return BadRequest(new
            {
                success = false,
                message = "Password and Confirm Password mismatch"
            });
        var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Username == registrationDto.Username);
        if (user != null)
            return BadRequest(new
            {
                success = false,
                message = "Username already exists"
            });
        var hashedPassword = _hashing.HashPassword(registrationDto.Password);
        var newUser = new UserModel()
        {
            Username = registrationDto.Username,
            Name = registrationDto.Name,
            Password = hashedPassword,
            Role = registrationDto.Role
        };
        await _appDbContext.Users.AddAsync(newUser);
        await _appDbContext.SaveChangesAsync();
        var accessToken = GenerateToken(newUser);
        return Ok(new
        {
            success = true,
            message = "User has been created",
            accessToken,
            data = newUser
        });
    }


    async private Task<UserModel> AuthenticateUser(LoginDTO loginDto)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(
            user => user.Username == loginDto.Username);
        if(user != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            return user;
        return null;
    }

    private string GenerateToken(UserModel user)
    {
        var secrectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecrectKey"]));
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Role),
        };
        var accessToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
        //var refreshToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(24), signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
}