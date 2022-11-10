using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.DataAccess
{
    public class CommentsRepository : Repository<Comment>
    {
        public CommentsRepository(VeryGoodNewsPortalContext context) : base(context)
        {
            
        }
    }
}
