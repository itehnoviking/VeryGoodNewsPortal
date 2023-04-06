using MediatR;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;

public class CreateCommentCommand : IRequest<bool>
{
    public CreateCommentCommand(CreateCommentDto dto)
    {
        Id = dto.Id;
        Text = dto.Text;
        CreationDate = dto.CreationDate;
        UserId = dto.UserId;
        ArticleId = dto.ArticleId;
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CreationDate { get; set; }
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
}