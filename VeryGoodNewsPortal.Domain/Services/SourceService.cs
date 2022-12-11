using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;

namespace VeryGoodNewsPortal.Domain.Services
{
    public class SourceService : ISourceService
    {
        private readonly ILogger<SourceService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RssUrlsFromSourceDto>> GetRssUrlsAsync()
        {
            try
            {
                var result = await _unitOfWork.Sources
                    .Get()
                    .Select(source => _mapper.Map<RssUrlsFromSourceDto>(source))
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return null;
            }
        }

        public async Task<Guid> GetSourceByUrl(string url)
        {
            var domain = string
                .Join(".", new Uri(url).Host
                .Split('.')
                .TakeLast(2)
                .ToList());


            return (await _unitOfWork.Sources
                .Get()
                .FirstOrDefaultAsync(source => source.SiteUrl.Contains(domain)))?.Id
                ??
                Guid.Empty;

        }

        public async Task<IEnumerable<SourceNameAndIdDto>> GetSourceNameAndId()
        {
            return await _unitOfWork.Sources
                .Get()
                .Select(source => _mapper.Map<SourceNameAndIdDto>(source))
                .ToListAsync();
        }
    }
}
