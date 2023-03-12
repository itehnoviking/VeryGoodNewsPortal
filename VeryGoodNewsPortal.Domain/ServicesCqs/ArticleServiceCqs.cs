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



        public async Task<ArticleDto> GetArticleByIdAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetArticleByIdQuery(id), new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesAsync(int? page)
        {
            try
            {
                if (page > 0 && page != null)
                {
                    return await GetArticlesByPageAsync(Convert.ToInt32(page));
                }

                return await _mediator.Send(new GetAllPositivityArticlesQuery(), new CancellationToken());
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        private async Task<IEnumerable<ArticleDto>> GetArticlesByPageAsync(int page)
        {
            try
            {
                var size = Convert.ToInt32(_configuration["ApplicationVariables:PageSize"]);


                return await _mediator.Send(new GetPositivityArticlesByPageQuery(size, page), new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
