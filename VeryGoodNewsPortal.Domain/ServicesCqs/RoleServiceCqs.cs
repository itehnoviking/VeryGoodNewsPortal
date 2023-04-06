using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Cqs.Models.Queries.RoleQueries;

namespace VeryGoodNewsPortal.Domain.ServicesCqs;

public class RoleServiceCqs : IRoleServiceCqs
{
    private readonly ILogger<RoleServiceCqs> _logger;
    private readonly IMediator _mediator;

    public RoleServiceCqs(ILogger<RoleServiceCqs> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<bool> IsAdminByIdUser(string userId)
    {
        try
        {
            return await _mediator.Send(new IsAdminByIdUserQuery(Guid.Parse(userId)), new CancellationToken());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}