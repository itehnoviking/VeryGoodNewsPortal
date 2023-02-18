using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System.Collections.Concurrent;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Domain.Services;
using VeryGoodNewsPortal.Models;

namespace VeryGoodNewsPortal.Controllers
{
    [Authorize(Roles = "User, Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;
        private readonly IRssService _rssService;
        private readonly IHtmlParserService _htmlParserService;
        private readonly IConfiguration _configuration;
        private readonly IPositivityGradeService _positivityGradeService;

        private readonly int _pageSize;

        public ArticleController(IArticleService articleService, IMapper mapper, ISourceService sourceService, IRssService rssService, IHtmlParserService htmlParserService, IConfiguration configuration, IPositivityGradeService positivityGradeService)
        {
            _articleService = articleService;
            _mapper = mapper;
            _sourceService = sourceService;
            _rssService = rssService;
            _htmlParserService = htmlParserService;
            _configuration = configuration;
            _positivityGradeService = positivityGradeService;

            _pageSize = Convert.ToInt32(_configuration["ApplicationVariables:PageSize"]);
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            try
            { 

                var pageAmount = Convert.ToInt32(Math.Ceiling((double)(await _articleService.GetAllArticlesAsync()).Count() / _pageSize));

                var articles = (await _articleService.GetArticleByPageAsync(page - 1))
                .Select(article => _mapper.Map<ArticleListItemViewModel>(article))
                .OrderByDescending(article => article.CreationDate).ToList();

                //var articles = (await _articleService.GetAllArticlesAsync())
                //    .Where(article => !String.IsNullOrWhiteSpace(article.Body))
                //    .Select(articles => _mapper.Map<ArticleListItemViewModel>(articles))
                //    .OrderByDescending(article => article.CreationDate)
                //    .ToList();

                if (articles.Any())
                {
                    var model = new ArticleIndexViewModel()
                    {
                        ArticleLists = articles,
                        PagesAmount = pageAmount
                    };

                    return View(model);
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody] SearchArticlesModel model)
        {
            var searchData = (await _articleService.GetArticleByNameAsync(model.SearchText))
                .Select(article => _mapper.Map<ArticleListItemViewModel>(article))
                .OrderByDescending(article => article.CreationDate)
                .ToList();

            return View("SearchPartial", searchData);
        }

        public async Task<IActionResult> GetTitles(string title)
        {
            var titles = await _articleService.GetAllArticlesTitlesAsync();

            return Ok(titles.Where(s => s.Contains(title, StringComparison.InvariantCultureIgnoreCase)).ToArray());
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            try
            {
                var article = await _articleService.GetArticleWitchSourceNameAndComments(id);

                var resultModel = _mapper.Map<ArticleWithSourceNameAndCommentsViewModel>(article);

                if (article != null)
                {
                    return View(resultModel);
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }


        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            try
            {
                var article = await _articleService.GetArticleAsync(id);
                var viewModel = _mapper.Map<ArticleEditViewModel>(article);

                if (article != null)
                {
                    return View(viewModel);
                }

                else
                {
                    throw new ArgumentException();
                }

            }
            catch (Exception e)
            {
                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditViewModel viewModel)
        {
            try
            {
                var entity = await _articleService.GetArticleAsync(viewModel.Id);

                await _articleService.UpdateArticle(_mapper.Map(viewModel, entity));

                if (viewModel != null)
                {
                    return RedirectToAction("Index", "Article");
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var article = await _articleService.GetArticleAsync(id);
                var viewModel = _mapper.Map<ArticleDeleteViewModel>(article);

                if (article != null)
                {
                    return View(viewModel);
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {

                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }

            

            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ArticleDeleteViewModel viewModel)
        {
            try
            {
                await _articleService.DeleteArticle(_mapper.Map<ArticleDto>(viewModel));

                if (viewModel != null)
                {
                    return RedirectToAction("Index", "Article");
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {

                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var sources = await _sourceService.GetSourceNameAndId();

                var viewModel = new ArticleCreateViewModel()
                {
                    SourceNameAndIdModels = sources.Select(source => new SelectListItem(source.Name, source.Id.ToString()))
                };

                if (viewModel != null)
                {
                    return View(viewModel);
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateViewModel viewModel)
        {
            try
            {
                await _articleService.CreateArticle(_mapper.Map<ArticleDto>(viewModel));

                if (viewModel != null)
                {
                    return RedirectToAction("Index", "Article");
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"{e.Message} \n Stack trace:{e.StackTrace}");

                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetNewsFromSources()
        {
            try
            {
                await _rssService.AggregateArticleDataFromRssSources();

                return RedirectToAction("Index", "Article");
            }
            catch (Exception ex)
            {
                var exMessage = string.Format(_configuration.GetSection("ApplicationVariables")["LogErrorMessageFormat"],
                    ex.Message,
                    ex.StackTrace);

                _logger.LogError(ex, exMessage);

                return StatusCode(500, new
                {
                    ex.Message
                });
            }
        }
    }
}
