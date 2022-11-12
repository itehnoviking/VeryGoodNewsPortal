using AutoMapper;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.Models;

namespace VeryGoodNewsPortal.Mappers
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<Source, SourceNameAndIdDTO>().ReverseMap();

            CreateMap<SourceNameAndIdDTO, SourceNameAndIdModel>().ReverseMap();
        }
    }
}
