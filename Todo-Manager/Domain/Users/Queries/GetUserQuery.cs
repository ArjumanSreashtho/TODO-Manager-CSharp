using MediatR;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Queries;

public class GetUserQuery : IRequest<UserTaskDTO>
{
    public string Username { get; set; }
}