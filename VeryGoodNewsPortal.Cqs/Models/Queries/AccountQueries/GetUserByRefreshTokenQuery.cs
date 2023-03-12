using MediatR;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.AccountQueries
{
    public class GetUserByRefreshTokenQuery : IRequest<UserDto>
    {
        public GetUserByRefreshTokenQuery(string token)
        {
            Token = token;
        }

        public string Token { get; set; }
    }
}
