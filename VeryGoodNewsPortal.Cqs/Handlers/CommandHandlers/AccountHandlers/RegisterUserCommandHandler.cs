using AutoMapper;
using MediatR;
using VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Cqs.Handlers.CommandHandlers.AccountHandlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly VeryGoodNewsPortalContext _database;

    public RegisterUserCommandHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RegisterUserCommand command, CancellationToken token)
    {
        var user = _mapper.Map<User>(command);

        await _database.Users.AddAsync(user, token);
        await _database.SaveChangesAsync(cancellationToken: token);

        return true;
    }
}