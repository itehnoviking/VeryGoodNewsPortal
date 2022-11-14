using AutoMapper;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.Models;

namespace VeryGoodNewsPortal.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDTO>().ReverseMap();

            CreateMap<ArticleDTO, ArticleListItemViewModel>();

            CreateMap<ArticleDTO, ArticleDetailViewModel>().ReverseMap();

            CreateMap<ArticleDTO, ArticleDeleteViewModel>().ReverseMap();

            CreateMap<ArticleCreateViewModel, ArticleDTO>();

            CreateMap<ArticleDTO, ArticleWithSourceNameAndCommentsViewModel>();

            CreateMap<ArticleDTO, ArticleEditViewModel>().ReverseMap();
        }
    }
}
