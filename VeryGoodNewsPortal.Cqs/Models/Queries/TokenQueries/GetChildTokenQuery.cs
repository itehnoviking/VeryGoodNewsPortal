using MediatR;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.TokenQueries;

public class GetChildTokenQuery : IRequest<RefreshTokenDto>
{
    public GetChildTokenQuery(string token)
    {
        Token = token;
    }
    public string Token { get; set; }

}