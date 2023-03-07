using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Todo_Manager.Data;
using Todo_Manager.DTO.Authentication;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Auth.Queries.Handlers;

public class GetAuthTokenHandler : IRequestHandler<GetAuthTokenQuery, string>
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfiguration _config;

    public GetAuthTokenHandler(AppDbContext appDbContext, IConfiguration config)
    {
        _appDbContext = appDbContext;
        _config = config;
    }
    
    public async Task<string> Handle(GetAuthTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await AuthenticateUser(request.LoginDTO);
        if (user == null)
            throw new CustomException("Invalid username or password", 400);
        var accessToken = GenerateToken(user);
        return accessToken;
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