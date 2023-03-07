using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Queries.Handlers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<RetrievedUserDTO>>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    public GetAllUsersHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    public async Task<List<RetrievedUserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var userList = await _appDbContext.Users.Where(user => user.Name.Contains(request.Search)).OrderByDescending(user => user.UpdatedAt).Skip((request.Page - 1) * request.Total).Take(request.Total).Select(user => _mapper.Map<RetrievedUserDTO>(user)).ToListAsync();
        return userList;
    }
}