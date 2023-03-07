using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;

namespace Todo_Manager.Domain.Users.Commands.Handlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, RetrievedUserDTO>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    
    public UpdateUserHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<RetrievedUserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FindAsync(request.Id);

        if (user == null)
            throw new CustomException("Not found", 404);
        
        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Username == request.UpdateUserDto.Username);
        
        if (existingUser != null && user.Username != request.UpdateUserDto.Username)
            throw new CustomException("Username already exists", 400);
        
        user.Name = request.UpdateUserDto.Name != null ? request.UpdateUserDto.Name : user.Name;
        user.Username = request.UpdateUserDto.Username != null ? request.UpdateUserDto.Username : user.Username;

        var userResponse = _mapper.Map<RetrievedUserDTO>(user);
        return userResponse;
    }
}