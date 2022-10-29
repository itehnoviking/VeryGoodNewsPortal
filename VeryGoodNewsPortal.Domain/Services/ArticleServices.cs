using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Domain.Services
{
    public class ArticleServices : IArticleServices
    {
        private readonly VeryGoodNewsPortalContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleServices(VeryGoodNewsPortalContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        
    }
}