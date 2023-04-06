using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;

public interface ICommentServiceCQS
{
    Task CreateAsync(CreateCommentDto dto);
    Task<object> GetByIdAsync(Guid id);
    Task EditAsync(object obj);
    Task DeleteAsync(Guid id);

}