using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class SourceServices : ISourceServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SourceServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SourceNameAndIdDTO>> GetSourceNameAndId()
        {
            return await _unitOfWork.Sources
                .Get()
                .Select(source => _mapper.Map<SourceNameAndIdDTO>(source))
                .ToListAsync();
        }
    }
}
