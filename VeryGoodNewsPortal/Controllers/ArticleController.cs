using AutoMapper;
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
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;
        private readonly IRssService _rssService;
        private readonly IHtmlParserService _htmlParserService;
        private readonly IConfiguration _configuration;

        public ArticleController(IArticleService articleService, IMapper mapper, ISourceService sourceService, IRssService rssService, IHtmlParserService htmlParserService, IConfiguration configuration)
        {
            _articleService = articleService;
            _mapper = mapper;
            _sourceService = sourceService;
            _rssService = rssService;
            _htmlParserService = htmlParserService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var articles = (await _articleService.GetAllArticlesAsync())
                    .Where(article => !String.IsNullOrWhiteSpace(article.Body))
                    .Select(articles => _mapper.Map<ArticleListItemViewModel>(articles))
                    .OrderByDescending(article => article.CreationDate)
                    .ToList();

                if (articles.Any())
                {
                    return View(articles);
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
                var rssUrls = await _sourceService.GetRssUrlsAsync();

                var concurrentDictionary = new ConcurrentDictionary<string, RssArticleDto>();

                var result = Parallel.ForEach(rssUrls, dto =>
                {
                    _rssService.GetArticlesInfoFromRss(dto.RssUrl)
                          .AsParallel()
                          .ForAll(articleDto => concurrentDictionary.TryAdd(articleDto.Url, articleDto));
                });

                var extArticlesUrls = await _articleService.GetAllExistingArticleUrls();

                Parallel.ForEach(extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)),
                    s => concurrentDictionary.Remove(s, out var dto));

                //var groupedRssArticle = concurrentDictionary.GroupBy(pair => _sourceService.GetSourceByUrl(pair.Key).Result);
                //foreach (var url in extArticlesUrls.Where(url => concurrentDictionary.ContainsKey(url)))
                //{
                //    concurrentDictionary.TryRemove(url, out var dto);

                //}


                foreach (var rssArticleDto in concurrentDictionary)
                {
                    var body = await _htmlParserService.GetArticleContentFromUrlAsync(rssArticleDto.Key);
                }

                return Ok();
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
