using MediatR;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.ArticleQueries;

public class GetAllArticlesQuery : IRequest<IEnumerable<ArticleDto>>
{

}