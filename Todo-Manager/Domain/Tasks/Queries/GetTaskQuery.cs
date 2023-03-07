using MediatR;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Queries;

public class GetTaskQuery : IRequest<TaskDTO>
{
    public Guid Id { get; set; }
}