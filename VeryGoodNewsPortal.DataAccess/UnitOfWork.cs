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
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Source> _sourceRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public UnitOfWork(VeryGoodNewsPortalContext db, 
            IRepository<Article> articleRepository, 
            IRepository<Role> roleRepository, 
            IRepository<User> userRepository, 
            IRepository<Source> sourceRepository, 
            IRepository<Comment> commentRepository, 
            IRepository<UserRole> userRoleRepository, 
            IRepository<RefreshToken> refreshTokenRepository)
        {
            _db = db;
            _articleRepository = articleRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _sourceRepository = sourceRepository;
            _commentRepository = commentRepository;
            _userRoleRepository = userRoleRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public IRepository<Article> Articles => _articleRepository;
        public IRepository<Role> Roles => _roleRepository;
        public IRepository<User> Users => _userRepository;
        public IRepository<Source> Sources => _sourceRepository;
        public IRepository<Comment> Comments => _commentRepository;
        public IRepository<UserRole> UserRoles => _userRoleRepository;
        public IRepository<RefreshToken> RefreshTokens => _refreshTokenRepository;

        public async Task<int> Commit()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
            _articleRepository.Dispose();
            _commentRepository.Dispose();
            _roleRepository.Dispose();
            _sourceRepository.Dispose();
            _userRepository.Dispose();
            _userRoleRepository.Dispose();
            _refreshTokenRepository.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
