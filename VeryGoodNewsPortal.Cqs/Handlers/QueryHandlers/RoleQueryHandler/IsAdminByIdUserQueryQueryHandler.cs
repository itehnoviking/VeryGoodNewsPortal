using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Queries.AccountQueries;
using VeryGoodNewsPortal.Cqs.Models.Queries.RoleQueries;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.RoleQueryHandler;

public class IsAdminByIdUserQueryQueryHandler : IRequestHandler<IsAdminByIdUserQuery, bool>
{
    private readonly VeryGoodNewsPortalContext _database;

    public IsAdminByIdUserQueryQueryHandler(VeryGoodNewsPortalContext database)
    {
        _database = database;
    }

    public async Task<bool> Handle(IsAdminByIdUserQuery query, CancellationToken token)
    {
        var userRoleArray = await _database.UserRoles
            .AsNoTracking()
            .Where(u => u.UserId.Equals(query.UserId))
            .Select(u => u.RoleId)
            .ToArrayAsync(cancellationToken: token);

        var isAdmin = await _database.Roles
            .AsNoTracking()
            .Where(r => userRoleArray.Contains(r.Id) && r.Name.Contains("Admin"))
            .AnyAsync(cancellationToken:token);

        return isAdmin;
    }
}