using MediatR;
using Todo_Manager.Data;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Commands.Handlers;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, TaskModel>
{
    private readonly AppDbContext _appDbContext;
    public UpdateTaskHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TaskModel> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _appDbContext.Tasks.FindAsync(request.Id);
        if (task == null)
            throw new CustomException("Not found", 404);
        task.Title = request.UpdateTask != null ? request.UpdateTask.Title : task.Title;
        task.Description = request.UpdateTask.Description != null ? request.UpdateTask.Description : task.Description;
        task.Completed = request.UpdateTask.Completed != null ? request.UpdateTask.Completed : task.Completed;
        await _appDbContext.SaveChangesAsync();
        return task;
    }
}