using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesWebApi
{
    public interface IJwtService
    {
        public string GenerateJwtToken(UserDto user);
        public Guid? ValidateJwtToken(string token);
        public RefreshTokenDto GenerateRefreshToken(string ipAddress);
    }

    
}
