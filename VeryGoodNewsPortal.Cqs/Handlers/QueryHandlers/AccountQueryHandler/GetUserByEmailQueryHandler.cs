using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Queries.AccountQueries;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.AccountQueryHandler;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto>
{
    private readonly VeryGoodNewsPortalContext _database;
    private readonly IMapper _mapper;

    public GetUserByEmailQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByEmailQuery request, CancellationToken token)
    {
        var user = await _database.Users
            .AsNoTracking()
            .Where(user => user.NormalizedEmail.Equals(request.Email))
            .Select(user => _mapper.Map<UserDto>(user))
            .FirstOrDefaultAsync(cancellationToken:token);

        return user;

    }
}