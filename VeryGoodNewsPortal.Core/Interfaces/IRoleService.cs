using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface IRoleService
    {
        Task<Guid> GetRoleIdByNameAsync(string name);
        Task<string> GetRoleNameByIdAsync(Guid roleId);
        Task<Guid> CreateRole(string name);
    }
}
