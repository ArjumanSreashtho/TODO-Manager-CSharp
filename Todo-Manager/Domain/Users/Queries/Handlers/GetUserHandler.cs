using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;
using Todo_Manager.Helper;

namespace Todo_Manager.Domain.Users.Queries.Handlers;

public class GetUserHandler : IRequestHandler<GetUserQuery, UserTaskDTO>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public GetUserHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    
    public async Task<UserTaskDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _appDbContext.Users
            .Where(user => user.Username == request.Username)
            .Include(user => user.UserTasks)
            .ThenInclude(userTask => userTask.Task)
            .FirstOrDefaultAsync();
        if (user == null)
            throw new CustomException("Not found", 404);
        var response = _mapper.Map<UserTaskDTO>(user);
        return response;
    }
}