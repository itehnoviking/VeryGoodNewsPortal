using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs
{
    public interface IArticleServiceCqs
    {
        Task<ArticleDto> GetArticleById(Guid id);
        Task<IEnumerable<ArticleDto>> GetAllArticles(int? page);
    }
}
