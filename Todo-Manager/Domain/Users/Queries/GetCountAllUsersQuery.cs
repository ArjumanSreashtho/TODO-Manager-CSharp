using MediatR;

namespace Todo_Manager.Domain.Users.Queries;

public class GetCountAllUsersQuery : IRequest<int>
{
    public string Search { get; set; } = "";
}