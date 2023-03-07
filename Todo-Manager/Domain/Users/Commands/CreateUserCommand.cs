using MediatR;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Commands;

public class CreateUserCommand : IRequest<RetrievedUserDTO>
{
    public CreateUserDTO User { get; set; }
}