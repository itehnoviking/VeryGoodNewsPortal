using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.DataAccess;

namespace VeryGoodNewsPortal.DataAccess
{
    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(VeryGoodNewsPortalContext db) : base(db)
        {
        }
    }
}
