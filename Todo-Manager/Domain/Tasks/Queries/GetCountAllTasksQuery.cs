using MediatR;

namespace Todo_Manager.Domain.Tasks.Queries;

public class GetCountAllTasksQuery : IRequest<int>
{
    public bool? Type { get; set; } = null;
    public string Search { get; set; } = "";
}