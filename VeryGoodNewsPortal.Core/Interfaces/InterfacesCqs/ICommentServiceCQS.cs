using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;

public interface ICommentServiceCQS
{
    Task CreateAsync(CreateCommentDto dto);
    Task<CommentDto> GetByIdAsync(Guid id);
    Task EditAsync(CommentDto dto);
    Task DeleteAsync(Guid id);

}