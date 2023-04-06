using MediatR;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.ArticleQueries;

public class GetAllArticlesByPageQuery : IRequest<IEnumerable<ArticleDto>>
{
    public GetAllArticlesByPageQuery(int pageSize, int pageNumber)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
    }

    public int PageSize { get; set; }
    public int PageNumber { get; set; }

}