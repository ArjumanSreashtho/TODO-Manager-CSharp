using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.Task;
using Todo_Manager.Helper;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Queries.Handlers;

public class GetTaskHandler : IRequestHandler<GetTaskQuery, TaskDTO>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    
    public GetTaskHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<TaskDTO> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _appDbContext.Tasks
            .Where(task => task.Id == request.Id)
            .Include(task => task.UserTasks)
            .ThenInclude(userTask => userTask.User)
            .FirstOrDefaultAsync();
        if (task == null)
            throw new CustomException("Not found", 404);
        var response = _mapper.Map<TaskDTO>(task);
        return response;
    }
}