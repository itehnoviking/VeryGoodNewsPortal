using AutoMapper;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.WebApi.Models.Requests;

namespace VeryGoodNewsPortal.WebApi.Mappers;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CreateCommentRequest, CreateCommentDto>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(guid => Guid.NewGuid()))
            .ForMember(dto => dto.Text, opt => opt.MapFrom(item => item.Text))
            .ForMember(dto => dto.UserId, opt => opt.MapFrom(item => item.UserId))
            .ForMember(dto => dto.ArticleId, opt => opt.MapFrom(item => item.ArticleId))
            .ForMember(dto => dto.CreationDate, opt => opt.MapFrom(date => DateTime.Now));

        CreateMap<CreateCommentCommand, Comment>();

        CreateMap<CommentDto, Comment>().ReverseMap();

        CreateMap<CommentDto, EditCommentCommand>().ReverseMap();
        CreateMap<Comment, EditCommentCommand>().ReverseMap();
    }
}