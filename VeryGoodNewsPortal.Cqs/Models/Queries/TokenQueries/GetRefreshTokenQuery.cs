using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.TokenQueries
{
    public class GetRefreshTokenQuery : IRequest<RefreshTokenDto>
    {
        public GetRefreshTokenQuery(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    }

}
