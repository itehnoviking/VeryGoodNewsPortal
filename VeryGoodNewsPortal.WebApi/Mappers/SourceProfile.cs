using AutoMapper;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.WebApi.Mappers
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<Source, RssUrlsFromSourceDto>()
                .ForMember(dto => dto.SourceId,
                    opt => opt.MapFrom(source => source.Id))
                .ForMember(dto => dto.RssUrl,
                    opt => opt.MapFrom(source => source.RssUrl));


        }
    }
}
