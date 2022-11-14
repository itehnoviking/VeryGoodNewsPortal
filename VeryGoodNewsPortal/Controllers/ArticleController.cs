using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Models;

namespace VeryGoodNewsPortal.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleServices _articleService;
        private readonly ISourceServices _sourceServices;
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleServices articleService, IMapper mapper, ISourceServices sourceServices)
        {
            _articleService = articleService;
            _mapper = mapper;
            _sourceServices = sourceServices;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var articles = (await _articleService.GetAllArticlesAsync())
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
                await _articleService.DeleteArticle(_mapper.Map<ArticleDTO>(viewModel));

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
                var sources = await _sourceServices.GetSourceNameAndId();

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
                await _articleService.CreateArticle(_mapper.Map<ArticleDTO>(viewModel));

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

    }
}
