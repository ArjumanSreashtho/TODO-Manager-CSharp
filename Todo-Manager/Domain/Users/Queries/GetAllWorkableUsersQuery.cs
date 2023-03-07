using MediatR;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Queries;

public class GetAllWorkableUsersQuery : IRequest<List<RetrievedUserDTO>>
{
    
}