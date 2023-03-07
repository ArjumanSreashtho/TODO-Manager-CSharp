using MediatR;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Commands;

public class UpdateTaskCommand : IRequest<TaskModel>
{
    public Guid Id { get; set; }
    public UpdateTaskDTO UpdateTask { get; set; }
}