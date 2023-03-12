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
    public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto>
    {
        private readonly IMapper _mapper;
        private readonly VeryGoodNewsPortalContext _database;

        public GetArticleByIdQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken token)
        {
            var article = await _database.Articles
                .AsNoTracking()
                .Where(article => article.Id.Equals(request.Id))
                .FirstOrDefaultAsync(cancellationToken:token);

            return _mapper.Map<ArticleDto>(article);
        }
    }
}
