using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Cqs.Models.Queries.AccountQueries;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.AccountQueryHandler;

public class GetRoleIdByNameQueryHandler : IRequestHandler<GetRoleIdByNameQuery, Guid>
{
    private readonly VeryGoodNewsPortalContext _database;
    private readonly IMapper _mapper;

    public GetRoleIdByNameQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(GetRoleIdByNameQuery query, CancellationToken token)
    {
        var roleId = await _database.Roles
            .AsNoTracking()
            .Where(roleName => roleName.Name.Equals(query.Name))
            .Select(role => role.Id)
            .FirstOrDefaultAsync(cancellationToken: token);

        return roleId;
    }
}