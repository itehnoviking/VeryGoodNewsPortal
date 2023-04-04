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
        Task<ArticleDto> GetArticleByIdAsync(Guid id);
        Task<IEnumerable<ArticleDto>> GetAllArticlesByPageAndRoleAsync(int? page, string role);
    }
}
