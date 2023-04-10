using AutoMapper;
using MediatR;
using VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Cqs.Handlers.CommandHandlers.AccountHandlers;

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, bool>
{
    private readonly VeryGoodNewsPortalContext _database;
    private readonly IMapper _mapper;


    public CreateUserRoleCommandHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateUserRoleCommand command, CancellationToken token)
    {
        var userRole = _mapper.Map<UserRole>(command);

        await _database.UserRoles.AddAsync(userRole, token);

        await _database.SaveChangesAsync(cancellationToken: token);

        return true;
    }
}