using MediatR;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Commands;

public class UpdateUserCommand : IRequest<RetrievedUserDTO>
{
    public Guid Id { get; set; }
    public UpdateUserDTO UpdateUserDto { get; set; }
}