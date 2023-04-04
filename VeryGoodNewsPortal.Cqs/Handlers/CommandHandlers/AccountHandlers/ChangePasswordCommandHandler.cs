using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.CommandHandlers.AccountHandlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly VeryGoodNewsPortalContext _database;

        public ChangePasswordCommandHandler(VeryGoodNewsPortalContext database)
        {
            _database = database;
        }

        public async Task<bool> Handle(ChangePasswordCommand command, CancellationToken token)
        {
            var user = await _database.Users.FindAsync(command.UserId);

            user.PasswordHash = command.NewPasswordHash;
            await _database.SaveChangesAsync(cancellationToken: token);

            return true;
        }
    }
}
