using AutoMapper;
using MediatR;
using VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Cqs.Handlers.CommandHandlers.CommentHandlers;

public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, bool>
{
    private readonly VeryGoodNewsPortalContext _database;
    private readonly IMapper _mapper;

    public EditCommentCommandHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<bool> Handle(EditCommentCommand request, CancellationToken token)
    {
        var comment = _mapper.Map<Comment>(request);

        _database.Comments.Update(comment);

        await _database.SaveChangesAsync(cancellationToken: token);

        return true;
    }
}