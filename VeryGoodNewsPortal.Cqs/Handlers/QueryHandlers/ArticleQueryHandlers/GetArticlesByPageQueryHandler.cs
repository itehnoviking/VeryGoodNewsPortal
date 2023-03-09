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
    public class GetArticlesByPageQueryHandler : IRequestHandler<GetArticlesByPageQuery, IEnumerable<ArticleDto>>
    {
        private readonly VeryGoodNewsPortalContext _database;
        private readonly IMapper _mapper;

        public GetArticlesByPageQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetArticlesByPageQuery query, CancellationToken token)
        {
            var articles = await _database.Articles
                .OrderByDescending(article => article.CreationDate)
                .Skip(query.PageNumber * query.PageSize)
                .Take(query.PageSize)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToArrayAsync(cancellationToken: token);

            return articles;
        }
    }
}