using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Todo_Manager.Data;
using Todo_Manager.DTO.Authentication;
using Todo_Manager.Helper;
using Todo_Manager.Models;
using Todo_Manager.Services.Interfaces;

namespace Todo_Manager.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfiguration _config;
    private readonly HashingPassword _hashingPassword;
    
    public AuthService(IConfiguration config, AppDbContext appDbContext, HashingPassword hashingPassword)
    {
        _appDbContext = appDbContext;
        _config = config;
        _hashingPassword = hashingPassword;
    }
    public async Task<string?> Login(LoginDTO loginDTO)
    {
        var user = await AuthenticateUser(loginDTO);
        if (user == null)
            throw new CustomException("Invalid username or password", 400);
        var accessToken = GenerateToken(user);
        return accessToken;
    }

    public async Task<(string, UserModel)?> Registration(RegistrationDTO registrationDto)
    {
        if (registrationDto.Password != registrationDto.ConfirmPassword)
            throw new CustomException("Password and Confirm Password mismatch", 400);
        var user = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Username == registrationDto.Username);
        if (user != null)
            throw new CustomException("Username already exists", 400);
        var hashedPassword = _hashingPassword.HashPassword(registrationDto.Password);
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
        return (accessToken, newUser);
    }
    
    private async Task<UserModel> AuthenticateUser(LoginDTO loginDto)
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
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Username)
        };
        var accessToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
}