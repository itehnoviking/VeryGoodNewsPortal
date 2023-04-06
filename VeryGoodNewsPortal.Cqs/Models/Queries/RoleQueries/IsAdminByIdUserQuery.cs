using MediatR;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.RoleQueries;

public class IsAdminByIdUserQuery : IRequest<bool>
{
    public IsAdminByIdUserQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; set; }
}