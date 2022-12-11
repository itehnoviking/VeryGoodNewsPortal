using AutoMapper;
using System.ServiceModel.Syndication;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.Models;

namespace VeryGoodNewsPortal.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();

            CreateMap<ArticleDto, ArticleListItemViewModel>();

            CreateMap<ArticleDto, ArticleDetailViewModel>().ReverseMap();

            CreateMap<ArticleDto, ArticleDeleteViewModel>().ReverseMap();

            CreateMap<ArticleCreateViewModel, ArticleDto>();

            CreateMap<ArticleDto, ArticleWithSourceNameAndCommentsViewModel>();

            CreateMap<ArticleDto, ArticleEditViewModel>().ReverseMap();

            CreateMap<SyndicationItem, RssArticleDto>()
                .ForMember(dto => dto.Url, opt => opt.MapFrom(item => item.Id))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(item => item.Title.Text))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(item => item.Summary.Text));

            CreateMap<RssArticleDto, ArticleDto>()
                .ForMember(dto => dto.Id, opt => opt.AddTransform(guid => Guid.NewGuid()))
                .ForMember(dto => dto.SourceUrl, opt => opt.MapFrom(item => item.Url))
                .ForMember(dto => dto.Title, opt => opt.MapFrom(item => item.Title))
                .ForMember(dto => dto.Description, opt => opt.MapFrom(item => item.Description));
        }
    }
}
