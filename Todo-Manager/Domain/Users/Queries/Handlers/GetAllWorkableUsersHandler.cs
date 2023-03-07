using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_Manager.Data;
using Todo_Manager.DTO.User;

namespace Todo_Manager.Domain.Users.Queries.Handlers;

public class GetAllWorkableUsersHandler : IRequestHandler<GetAllWorkableUsersQuery, List<RetrievedUserDTO>>
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    
    public GetAllWorkableUsersHandler(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }
    
    public async Task<List<RetrievedUserDTO>> Handle(GetAllWorkableUsersQuery request, CancellationToken cancellationToken)
    {
        var userList = await _appDbContext.Users.Select(user => _mapper.Map<RetrievedUserDTO>(user)).ToListAsync();
        return userList;
    }
}