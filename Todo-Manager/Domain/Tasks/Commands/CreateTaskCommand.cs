using MediatR;
using Todo_Manager.DTO.Task;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Commands;

public class CreateTaskCommand : IRequest<TaskModel>
{
    public CreateTaskDTO Task { get; set; }
}