using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Core.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Article> Articles { get; }
        IRepository<Role> Roles { get; }
        IRepository<User> Users { get; }
        IRepository<Source> Sources { get; }
        IRepository<Comment> Comments { get; }

        Task<int> Comit();
    }
}
