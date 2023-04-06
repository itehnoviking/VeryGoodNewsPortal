using AutoMapper;
using MediatR;
using VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;
using VeryGoodNewsPortal.Data;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Cqs.Handlers.CommandHandlers.CommentHandlers;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, bool>
{
    private readonly VeryGoodNewsPortalContext _database;
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateCommentCommand request, CancellationToken token)
    {
        var comment = _mapper.Map<Comment>(request);

        await _database.Comments
            .AddAsync(comment);

        await _database.SaveChangesAsync(cancellationToken: token);

        return true;
    }
}