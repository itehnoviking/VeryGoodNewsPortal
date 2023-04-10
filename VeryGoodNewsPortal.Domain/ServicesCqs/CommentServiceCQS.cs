using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;

namespace VeryGoodNewsPortal.Domain.ServicesCqs;

public class CommentServiceCQS : ICommentServiceCQS
{
    private readonly IMapper _mapper;
    private readonly ILogger<CommentServiceCQS> _logger;
    private readonly IMediator _mediator;

    public CommentServiceCQS(IMapper mapper, ILogger<CommentServiceCQS> logger, IMediator mediator)
    {
        _mapper = mapper;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task CreateAsync(CreateCommentDto dto)
    {
        try
        {
            var command = new CreateCommentCommand(dto);
            
            await _mediator.Send(command, new CancellationToken());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteCommentCommand(id), new CancellationToken());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public Task<object> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task EditAsync(object obj)
    {
        throw new NotImplementedException();
    }

    
}