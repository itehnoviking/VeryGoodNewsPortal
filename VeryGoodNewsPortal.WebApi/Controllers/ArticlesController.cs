using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.WebApi.Models.Responses;

namespace VeryGoodNewsPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleServiceCqs _articleServiceCqs;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleServiceCqs articleServiceCqs, ILogger<ArticlesController> logger)
        {
            _articleServiceCqs = articleServiceCqs;
            _logger = logger;
        }

        [HttpGet("{id}")]
        //[Authorize]
        [ProducesResponseType(typeof(ArticleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseErrorMessage), 500)]
        [ProducesResponseType(typeof(ResponseErrorMessage), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new ResponseErrorMessage { Message = "Identificator is incorrect" });
                }

                var article = await _articleServiceCqs.GetArticleById(id);

                if (article != null)
                {
                    return Ok(article);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ResponseErrorMessage { Message = ex.Message });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Get(GetArticleRequest request)
        //{
        //    try
        //    {
        //        if (!(string.IsNullOrEmpty(request.Name) && string.IsNullOrWhiteSpace(request.Name)))
        //        {
        //            var articles = await _articleService.GetArticleByNameAsync(request.Name);
        //            if (articles != null)
        //            {
        //                if (request.StartDate != null)
        //                {
        //                    articles.Where(dto => dto.CreationDate >= request.StartDate)
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw;
        //    }
        //}


    }
}
