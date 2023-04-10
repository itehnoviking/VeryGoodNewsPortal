using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Cors;
using VeryGoodNewsPortal.Core.DTOs;
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

                var article = await _articleServiceCqs.GetArticleByIdAsync(id);

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

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<ArticleDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseErrorMessage), 500)]
        [ProducesResponseType(typeof(ResponseNoContentMessage), 204)]
        public async Task<IActionResult> Get(int? page)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault().Value;

                var articles = await _articleServiceCqs.GetAllArticlesByPageAndRoleAsync(page, userId);

                if (articles == null || !articles.Any())
                {
                    return StatusCode(204, new ResponseNoContentMessage { Message = "No content" });
                }

                return Ok(articles);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new ResponseErrorMessage { Message = ex.Message });
            }
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<ArticleDto>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ResponseErrorMessage), 500)]
        //[ProducesResponseType(typeof(ResponseNoContentMessage), 204)]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var articles = await _articleServiceCqs.GetAllArticlesAsync();

        //        if (articles == null || !articles.Any())
        //        {
        //            return StatusCode(204, new ResponseNoContentMessage { Message = "No content" });
        //        }

        //        return Ok(articles);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return StatusCode(500, new ResponseErrorMessage { Message = ex.Message });
        //    }
        //}


    }
}
