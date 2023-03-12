using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Queries.TokenQueries;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.TokenQueryHandler
{
    public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, RefreshTokenDto>
    {
        private readonly IMapper _mapper;
        private readonly VeryGoodNewsPortalContext _database;

        public GetRefreshTokenQueryHandler(IMapper mapper, VeryGoodNewsPortalContext database)
        {
            _mapper = mapper;
            _database = database;
        }

        public async Task<RefreshTokenDto> Handle(GetRefreshTokenQuery request, CancellationToken token)
        {
            var refreshToken = await _database.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.Token.Equals(request.Token))
                .FirstOrDefaultAsync(cancellationToken: token);

            return _mapper.Map<RefreshTokenDto>(refreshToken);
        }
    }
}
