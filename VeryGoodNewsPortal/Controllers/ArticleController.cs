using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Models;

namespace VeryGoodNewsPortal.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleServices _articleService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleServices articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
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
                //todo added loger here 

                return BadRequest();
            }
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            try
            {
                var article = await _articleService.GetArticleAsync(id);

                var resultModel = _mapper.Map<ArticleDetailModel>(article);

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
                //todo added loger here 

                return BadRequest();
            }

            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var article = await _articleService.GetArticleAsync(id);
            var resultModel = _mapper.Map<ArticleDetailModel>(article);


            return View(resultModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleDetailModel model)
        {
            await _articleService.UpdateArticle(_mapper.Map<ArticleDTO>(model));

            return RedirectToAction("Index", "Article");
        } 
    }
}
