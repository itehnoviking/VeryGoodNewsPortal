using MediatR;
using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Cqs.Models.Queries.CommentQueries;

public class GetCommentByIdQuery : IRequest<CommentDto>
{
    public GetCommentByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}