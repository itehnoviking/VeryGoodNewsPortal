using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private readonly IRoleServiceCqs _roleServiceCqs;
        private readonly int _size;

        public ArticleServiceCqs(ILogger<ArticleServiceCqs> logger, IConfiguration configuration, IMediator mediator, IRoleServiceCqs roleServiceCqs)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
            _roleServiceCqs = roleServiceCqs;

            _size = Convert.ToInt32(_configuration["ApplicationVariables:PageSize"]);
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

        public async Task<IEnumerable<ArticleDto>> GetAllArticlesByPageAndRoleAsync(int? page, string userId)
        {
            try
            {

                //todo add role validation logic!

                if (await _roleServiceCqs.IsAdminByIdUser(userId))
                {
                    return await _mediator.Send(new GetAllArticlesByPageQuery(_size, Convert.ToInt32(page)), new CancellationToken());
                }

                if (page > 0 && page != null)
                {
                    return await _mediator.Send(new GetPositivityArticlesByPageQuery(_size, Convert.ToInt32(page)), new CancellationToken());
                }

                return await _mediator.Send(new GetAllPositivityArticlesQuery(), new CancellationToken());
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
    }
}
