using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface IArticleServices
    {
        Task<IList<ArticleDTO>> GetAllArticlesAsync();
        Task<ArticleDTO> GetArticleAsync(Guid id);

        Task UpdateArticle(ArticleDTO model);

        Task DeleteArticle(ArticleDTO model);

        Task CreateArticle(ArticleDTO model);
    }
}
