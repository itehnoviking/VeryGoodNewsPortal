using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Queries.CommentQueries;
using VeryGoodNewsPortal.Data;

namespace VeryGoodNewsPortal.Cqs.Handlers.QueryHandlers.CommentQueryHandlers;

public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentDto>
{
    private readonly VeryGoodNewsPortalContext _database;
    private readonly IMapper _mapper;

    public GetCommentByIdQueryHandler(VeryGoodNewsPortalContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(GetCommentByIdQuery request, CancellationToken token)
    {
        var comment = await _database.Comments
            .AsNoTracking()
            .Where(comment => comment.Id.Equals(request.Id))
            .FirstOrDefaultAsync(cancellationToken: token);

        return _mapper.Map<CommentDto>(comment);
    }
}