using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Cqs.Models.Commands.CommentCommand;
using VeryGoodNewsPortal.Cqs.Models.Queries.CommentQueries;

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

    public async Task<CommentDto> GetByIdAsync(Guid id)
    {
        try
        {
            var comment = await _mediator.Send(new GetCommentByIdQuery(id), new CancellationToken());

            return comment;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task EditAsync(CommentDto commentDto)
    {
        try
        {
            var command = _mapper.Map<EditCommentCommand>(commentDto);
            
            await _mediator.Send(command, new CancellationToken());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    
}