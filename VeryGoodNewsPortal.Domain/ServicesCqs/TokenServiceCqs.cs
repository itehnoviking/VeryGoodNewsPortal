

using MediatR;
using Microsoft.Extensions.Logging;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Cqs.Models.Queries.TokenQueries;

namespace VeryGoodNewsPortal.Domain.ServicesCqs
{
    public class TokenServiceCqs : ITokenServiceCqs
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TokenServiceCqs> _logger;

        public TokenServiceCqs(IMediator mediator, ILogger<TokenServiceCqs> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<RefreshTokenDto> GetRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var resultToken = await _mediator.Send(new GetRefreshTokenQuery(refreshToken), new CancellationToken());

                return resultToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<RefreshTokenDto> GetChildTokenAsync(RefreshTokenDto refreshToken)
        {
            try
            {
                var resultToken = await _mediator.Send(new GetChildTokenQuery(refreshToken.ReplaceByToken), new CancellationToken());

                return resultToken;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
