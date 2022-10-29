using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.Data;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VeryGoodNewsPortalContext _db;
        private readonly IRepository<Article> _articleRepository;

        public UnitOfWork(IRepository<Article> articleRepository, VeryGoodNewsPortalContext db)
        {
            _articleRepository = articleRepository;
            _db = db;
        }


        public IRepository<Article> Articles => _articleRepository;

        public object Roles { get; }

        public object Users { get; }

        public object Sources { get; }

        public object Comments { get; }



        public async Task<int> Comit()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
            _articleRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
