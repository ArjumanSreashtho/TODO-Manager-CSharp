using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;

namespace Todo_Manager.Domain.Tasks.Queries.Handlers;

public class GetCountAllTasksHandler : IRequestHandler<GetCountAllTasksQuery, int>
{
    private readonly AppDbContext _appDbContext;
    
    public GetCountAllTasksHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<int> Handle(GetCountAllTasksQuery request, CancellationToken cancellationToken)
    {
        var totalTasks = await _appDbContext.Tasks.Where(task => task.Title.Contains(request.Search) && (request.Type == null || request.Type == task.Completed)).CountAsync();
        return totalTasks;
    }
}