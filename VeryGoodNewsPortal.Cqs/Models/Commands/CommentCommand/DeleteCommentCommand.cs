using MediatR;

namespace VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;

public class DeleteCommentCommand : IRequest<bool>
{
    public DeleteCommentCommand(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}