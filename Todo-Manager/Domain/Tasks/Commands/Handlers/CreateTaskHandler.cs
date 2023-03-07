using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Todo_Manager.Data;
using Todo_Manager.DTO.Task;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Commands.Handlers;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskModel>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public CreateTaskHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<TaskModel> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        using (IDbContextTransaction transaction = await _appDbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var task = _mapper.Map<TaskModel>(request.Task);
                await _appDbContext.Tasks.AddAsync(task);
                foreach (var userId in request.Task.Users)
                {
                    await _appDbContext.UserTask.AddAsync(new UserTaskModel()
                    {
                        TaskId = task.Id,
                        UserId = userId
                    });
                }
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return task;
            }
            catch (Exception error)
            {
                await transaction.RollbackAsync();
                throw new CustomException(error.Message, 400);
            }
        }
    }
}