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
    public class GetAllArticlesQueryHandler : IRequestHandler<GetAllArticlesQuery, IEnumerable<ArticleDto>>
    {
        private readonly IMapper _mapper;
        private readonly VeryGoodNewsPortalContext _database;

        public GetAllArticlesQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllArticlesQuery request, CancellationToken token)
        {
            var articles = await _database.Articles
                .AsNoTracking()
                .OrderByDescending(article => article.CreationDate)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToArrayAsync(cancellationToken: token);

            return articles;

        }
    }
}
