using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface IAccountService
    {
        Task<bool> CheckUserWithThatEmailIsExistAsync(string email);
        Task<Guid> CreateUserAsync(string modelEmail);
        Task<int> SetRoleAsync(Guid userId, string roleName);
        Task<int> SetPasswordAsync(Guid userId, string password);
        Task<Guid?> GetUserIdByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(string email, string password);
        Task<IEnumerable<string>> GetRolesAsync(Guid userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByRefreshTokenAsync(string refreshToken);
        Task<UserDto> GetUserByIdAsync(Guid id);
    }
}
