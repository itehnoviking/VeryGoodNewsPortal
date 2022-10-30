using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Core.Interfaces.Data
{
    public interface IUnitOfWork/* : IDisposable*/
    {
        IRepository<Article> Articles { get; }
        object Roles { get; }
        object Users { get; }
        object Sources { get; }
        object Comments { get; }

        Task<int> Comit();
    }
}
