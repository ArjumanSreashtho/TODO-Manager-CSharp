using MediatR;
using Todo_Manager.DTO.Authentication;

namespace Todo_Manager.Domain.Auth.Queries;

public class GetAuthTokenQuery : IRequest<string>
{
    public LoginDTO LoginDTO { get; set; }
}