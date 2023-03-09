using MediatR;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.ArticleQueries
{
    public class GetArticlesByPageQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public GetArticlesByPageQuery(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}