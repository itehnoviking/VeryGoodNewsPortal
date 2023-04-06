using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Queries.ArticleQueries;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.ArticleQueryHandlers
{
    public class GetAllArticlesByPageQueryHandler : IRequestHandler<GetAllArticlesByPageQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly VeryGoodNewsPortalContext _database;

        public GetAllArticlesByPageQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllArticlesByPageQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .AsNoTracking()
                .OrderByDescending(article => article.CreationDate)
                .Skip(query.PageNumber * query.PageSize)
                .Take(query.PageSize)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToArrayAsync(cancellationToken: token);

            return articles;

        }
    }
}
