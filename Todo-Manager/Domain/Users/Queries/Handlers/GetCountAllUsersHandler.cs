using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;

namespace Todo_Manager.Domain.Users.Queries.Handlers;

public class GetCountAllUsersHandler : IRequestHandler<GetCountAllUsersQuery, int>
{
    private readonly AppDbContext _appDbContext;
    
    public GetCountAllUsersHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<int> Handle(GetCountAllUsersQuery request, CancellationToken cancellationToken)
    {
        var totalUser = await _appDbContext.Users.Where(user => user.Name.Contains(request.Search)).CountAsync();
        return totalUser;
    }
}