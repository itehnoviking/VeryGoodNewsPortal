﻿using AutoMapper;
using FirstMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Domain.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<int> UpdatePositivityGradeArticle(Guid articleId, int positivityGrade)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(articleId);

            await _unitOfWork.Articles.PatchAsync(articleId, new List<PatchModel>()
            {
                new PatchModel()
                {
                    PropertyName = "PositivityGrade",
                    PropertyValue = positivityGrade
                }
            });

            return await _unitOfWork.Commit();
        }

        public async Task<GuidAndTitleArticleDTO> GetArticleTitleWithoutPositivityGrade()
        {
            try
            {
                var article = (await _unitOfWork.Articles.FindBy(article => article.PositivityGrade.Equals(0))).FirstOrDefault();

                if (article != null && article.Title != null)
                {
                    return _mapper.Map<GuidAndTitleArticleDTO>(article);
                }

                else
                {
                    throw new ArgumentException();
                }

            }
            catch (Exception e)
            {
                //todo add logger here
                throw;
            }
        }

        public async Task<IList<ArticleDto>> GetAllArticlesAsync()
        {
            try
            {
                var listArticles = await _unitOfWork.Articles
                    .Get()
                    .Select(article => _mapper.Map<ArticleDto>(article)).ToListAsync();

                if (listArticles.Any())
                {
                    return listArticles;
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                //todo add logger here

                throw;
            }
        }

        public async Task<ArticleDto> GetArticleAsync(Guid id)
        {
            try
            {
                var article = await _unitOfWork.Articles
                    .GetByIdAsync(id);

                if (article != null)
                {
                    return _mapper.Map<ArticleDto>(article);
                }

                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception e)
            {
                //todo add logger
                throw;
            }
        }

        public async Task<ArticleDto> GetArticleWitchSourceNameAndComments(Guid id)
        {
            var articleWithSourceNameAndComments = await _unitOfWork.Articles
                .Get()
                .Where(article => article.Id.Equals(id))
                .Include(article => article.Source)
                .Include(article => article.Comments)
                .ThenInclude(comments => comments.User)
                .FirstOrDefaultAsync();


            return _mapper.Map<ArticleDto>(articleWithSourceNameAndComments);

        }

        public async Task UpdateArticle(ArticleDto model)
        {
            _unitOfWork.Articles.Update(_mapper.Map<Article>(model));
            await _unitOfWork.Commit();
        }

        public async Task DeleteArticle(ArticleDto model)
        {
            var entity = await _unitOfWork.Articles.GetByIdAsync(model.Id);

            _unitOfWork.Articles.Remove(entity);
            await _unitOfWork.Commit();
        }

        public async Task CreateArticle(ArticleDto model)
        {
            var entity = _mapper.Map<Article>(model);

            await _unitOfWork.Articles.AddAsync(entity);
            await _unitOfWork.Commit();
        }

        public async Task<List<string>> GetAllExistingArticleUrls()
        {
            return await _unitOfWork.Articles.Get().Select(article => article.SourceUrl).ToListAsync();
        }

        public async Task<int> InsertArticles(IEnumerable<ArticleDto> articles)
        {
            var entities = articles.Select(dto => _mapper.Map<Article>(dto)).ToArray();

            await _unitOfWork.Articles.AddRangeAsync(entities);

            return await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<ArticleDto>> GetArticleByPageAsync(int page)
        {
            var pageSize = Convert.ToInt32(_configuration["ApplicationVariables:PageSize"]);
            return await _unitOfWork.Articles
                .Get()
                .OrderByDescending(article => article.CreationDate)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToArrayAsync();
        }

        public async Task<IEnumerable<ArticleDto>> GetArticleByNameAsync(string name)
        {
            var articles = await _unitOfWork.Articles.Get()
                .Where(article => article.Title.Contains(name))
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToListAsync();

            return articles;

        }

        public async Task<string[]> GetAllArticlesTitlesAsync()
        {
            return await _unitOfWork.Articles.Get().Select(a => a.Title).ToArrayAsync();
        }
    }
}