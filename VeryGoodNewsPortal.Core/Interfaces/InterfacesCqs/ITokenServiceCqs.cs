using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs
{
    public interface ITokenServiceCqs
    {
        Task<RefreshTokenDto> GetRefreshTokenAsync(string refreshToken);
        Task<RefreshTokenDto> GetChildTokenAsync(RefreshTokenDto refreshToken);
    }
}
