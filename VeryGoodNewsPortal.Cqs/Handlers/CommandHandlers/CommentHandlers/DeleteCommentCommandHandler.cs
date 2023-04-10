using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.CommandHandlers.CommentHandlers;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
{
    private readonly VeryGoodNewsPortalContext _database;

    public DeleteCommentCommandHandler(VeryGoodNewsPortalContext database)
    {
        _database = database;
    }

    public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken token)
    {
        var comment = await _database.Comments.Where(c => c.Id.Equals(request.Id)).FirstOrDefaultAsync(cancellationToken: token);

        _database.Comments.Remove(comment);

        await _database.SaveChangesAsync(cancellationToken: token);

        return true;
    }
}