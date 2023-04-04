using MediatR;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.AccountQueries;


public class GetRoleIdByNameQuery : IRequest<Guid>
{
    public GetRoleIdByNameQuery(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
