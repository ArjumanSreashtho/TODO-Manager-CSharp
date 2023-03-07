using MediatR;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Commands;

public class DeleteTaskCommand : IRequest<TaskModel>
{
    public Guid Id { get; set; }
}