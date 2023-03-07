using MediatR;
using Todo_Manager.Data;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Commands.Handlers;

public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, TaskModel>
{
    private readonly AppDbContext _appDbContext;
    public DeleteTaskHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<TaskModel> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _appDbContext.Tasks.FindAsync(request.Id);
        if (task == null)
            throw new CustomException("Not found", 404);
        _appDbContext.Tasks.Remove(task);
        await _appDbContext.SaveChangesAsync();
        return task;
    }
}