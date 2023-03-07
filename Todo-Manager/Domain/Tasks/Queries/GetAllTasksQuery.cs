using MediatR;
using Todo_Manager.Models;

namespace Todo_Manager.Domain.Tasks.Queries;

public class GetAllTasksQuery : IRequest<List<TaskModel>>
{
    public bool? Type { get; set; } = null;
    public string Search { get; set; } = "";
    public int Page { get; set; } = 1;
    public int Total { get; set; } = 10;
}