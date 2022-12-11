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
            CreateMap<Source, SourceNameAndIdDto>().ReverseMap();

            CreateMap<SourceNameAndIdDto, SourceNameAndIdModel>().ReverseMap();

            CreateMap<Source, RssUrlsFromSourceDto>()
                .ForMember(dto => dto.SourceId,
                    opt => opt.MapFrom(source => source.Id))
                .ForMember(dto => dto.RssUrl,
                    opt => opt.MapFrom(source => source.RssUrl));
        }
    }
}
