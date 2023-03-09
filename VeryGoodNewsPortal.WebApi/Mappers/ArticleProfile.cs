using AutoMapper;
using System;
using System.ServiceModel.Syndication;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.WebApi.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {

            CreateMap<Article, ArticleDto>();

        }
    }
}
