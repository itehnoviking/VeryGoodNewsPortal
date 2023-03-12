using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Queries.TokenQueries;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.TokenQueryHandler;

public class GetChildTokenQueryHandler : IRequestHandler<GetChildTokenQuery, RefreshTokenDto>
{
    private readonly VeryGoodNewsPortalContext _database;
    private readonly IMapper _mapper;
    public GetChildTokenQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }


    public async Task<RefreshTokenDto> Handle(GetChildTokenQuery request, CancellationToken token)
    {
        var refreshToken = await _database.RefreshTokens
            .AsNoTracking()
            .Where(rt => rt.ReplaceByToken.Equals(request.Token))
            .FirstOrDefaultAsync(cancellationToken:token);

        return _mapper.Map<RefreshTokenDto>(refreshToken);
    }
}