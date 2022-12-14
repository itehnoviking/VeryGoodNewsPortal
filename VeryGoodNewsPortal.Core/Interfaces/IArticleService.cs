using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface IArticleService
    {
        Task<IList<ArticleDto>> GetAllArticlesAsync();
        Task<ArticleDto> GetArticleAsync(Guid id);

        Task UpdateArticle(ArticleDto model);

        Task DeleteArticle(ArticleDto model);

        Task CreateArticle(ArticleDto model);

        Task<ArticleDto> GetArticleWitchSourceNameAndComments(Guid id);

        Task<List<string>> GetAllExistingArticleUrls();

        Task<int> InsertArticles(IEnumerable<ArticleDto> articles);
    }
}
