using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesWebApi
{
    public interface ITokenService
    {
        Task RevokeTokenAsync(string token, string getIpAddress);
        Task<JwtAuthDto> RefreshTokenAsync(string? token, string getIpAddress);
        Task<JwtAuthDto> GetTokenAsync(LoginDto request, string getIpAddress);
    }
}
