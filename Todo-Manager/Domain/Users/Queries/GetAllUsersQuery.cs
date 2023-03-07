using MediatR;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Queries;

public class GetAllUsersQuery : IRequest<List<RetrievedUserDTO>>
{
    public string Search { get; set; } = "";
    public int Page { get; set; } = 1;
    public int Total { get; set; } = 10;
}