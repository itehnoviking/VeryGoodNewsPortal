using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Cqs.Models.Queries.ArticleQueries;

namespace VeryGoodNewsPortal.Domain.ServicesCqs
{
    public class ArticleServiceCqs : IArticleServiceCqs
    {
        private readonly ILogger<ArticleServiceCqs> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ArticleServiceCqs(ILogger<ArticleServiceCqs> logger, IConfiguration configuration, IMediator mediator)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
        }

       

        public async Task<ArticleDto> GetArticleById(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetArticleByIdQuery(id),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticles(int? page)
        {
            throw new NotImplementedException();
        }
    }
}
