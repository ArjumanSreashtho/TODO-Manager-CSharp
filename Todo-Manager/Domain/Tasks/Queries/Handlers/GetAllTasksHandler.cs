using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Queries.Handlers;

public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, List<TaskModel>>
{
    private readonly AppDbContext _appDbContext;
    public GetAllTasksHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<List<TaskModel>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var tasksList = await _appDbContext.Tasks.Where(task => task.Title.Contains(request.Search) && (request.Type == null || task.Completed == request.Type)).OrderByDescending(task => task.CreatedAt).Skip((request.Page - 1) * request.Total).Take(request.Total).ToListAsync(cancellationToken: cancellationToken);
        return tasksList;
    }
}