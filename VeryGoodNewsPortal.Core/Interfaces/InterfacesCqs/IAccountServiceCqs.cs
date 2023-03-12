using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs
{
    public interface IAccountServiceCqs
    {
        Task<UserDto> GetUserByRefreshTokenAsync(string token);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<bool> CheckPasswordByEmailAsync(string email, string password);
    }
}
