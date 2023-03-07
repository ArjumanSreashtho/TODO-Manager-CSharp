using MediatR;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Commands;

public class DeleteUserCommand : IRequest<RetrievedUserDTO>
{
    public Guid Id { get; set; }
}