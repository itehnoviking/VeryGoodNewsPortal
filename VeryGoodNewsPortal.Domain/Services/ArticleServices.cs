﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Domain.Services
{
    public class ArticleServices : IArticleServices
    {
        //private readonly VeryGoodNewsPortalContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleServices(/*VeryGoodNewsPortalContext context,*/ IUnitOfWork unitOfWork, IMapper mapper)
        {
            //_context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<ArticleDTO>> GetAllArticlesAsync()
        {
            try
            {
                var listArticles = await _unitOfWork.Articles
                    .Get()
                    .Select(article => _mapper.Map<ArticleDTO>(article)).ToListAsync();

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

        public async Task<ArticleDTO> GetArticleAsync(Guid id)
        {
            try
            {
                var article = await _unitOfWork.Articles
                    .GetByIdAsync(id);

                if (article != null)
                {
                    return _mapper.Map<ArticleDTO>(article);
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

        public async Task UpdateArticle(ArticleDTO model)
        {
            _unitOfWork.Articles.Update(_mapper.Map<Article>(model));
            await _unitOfWork.Comit();
        }

        public async Task DeleteArticle(ArticleDTO model)
        {
            var entity = await _unitOfWork.Articles.GetByIdAsync(model.Id);

            _unitOfWork.Articles.Remove(entity);
            await _unitOfWork.Comit();
        }

        public async Task CreateArticle(ArticleDTO model)
        {
            var entity = _mapper.Map<Article>(model);

            await _unitOfWork.Articles.AddAsync(entity);
            await _unitOfWork.Comit();
        }
    }
}