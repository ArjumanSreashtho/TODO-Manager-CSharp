using AutoMapper;
using MediatR;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;

namespace Todo_Manager.Domain.Users.Commands.Handlers;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, RetrievedUserDTO>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public DeleteUserHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    
    public async Task<RetrievedUserDTO> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users.FindAsync(request.Id);
        if (user == null)
            throw new CustomException("Not found", 404);
        _appDbContext.Remove(user);
        await _appDbContext.SaveChangesAsync();
        var userResponse = _mapper.Map<RetrievedUserDTO>(user);
        return userResponse;
    }
}