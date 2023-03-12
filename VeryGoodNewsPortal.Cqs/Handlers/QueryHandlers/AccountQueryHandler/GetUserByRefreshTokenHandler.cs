using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Queries.AccountQueries;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.AccountQueryHandler
{
    public class GetUserByRefreshTokenHandler : IRequestHandler<GetUserByRefreshTokenQuery, UserDto>
    {
        private readonly VeryGoodNewsPortalContext _database;
        private readonly IMapper _mapper;

        public GetUserByRefreshTokenHandler(VeryGoodNewsPortalContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByRefreshTokenQuery request, CancellationToken token)
        {
            var refreshToken = await _database.RefreshTokens
                .AsNoTracking()
                .Where(rt => rt.Token.Equals(request.Token))
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(cancellationToken: token);

            return _mapper.Map<UserDto>(refreshToken.User);
        }
    }
}
