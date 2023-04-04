using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public ChangePasswordCommand(Guid userId, string newPasswordHash)
        {
            UserId = userId;
            NewPasswordHash = newPasswordHash;
        }

        public Guid UserId { get; set; }
        public string NewPasswordHash { get; set; }
    }
}
