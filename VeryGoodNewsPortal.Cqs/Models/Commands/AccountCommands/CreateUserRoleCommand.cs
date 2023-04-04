using MediatR;

namespace VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands;

public class CreateUserRoleCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
}