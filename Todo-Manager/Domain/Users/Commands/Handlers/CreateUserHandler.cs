using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.Domain.Tasks.Commands.Handlers;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Users.Commands.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, RetrievedUserDTO>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    
    public CreateUserHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<RetrievedUserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(exUser => exUser.Username == request.User.Username);
        if (existingUser != null)
            throw new CustomException("Username already exists", 400);
        var newUser = _mapper.Map<UserModel>(request.User);

        await _appDbContext.Users.AddAsync(newUser);
        await _appDbContext.SaveChangesAsync();
        var userResponse = _mapper.Map<RetrievedUserDTO>(newUser);
        return userResponse;
    }
}