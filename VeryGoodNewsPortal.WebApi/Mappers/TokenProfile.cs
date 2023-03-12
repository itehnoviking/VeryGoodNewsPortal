using AutoMapper;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.WebApi.Models.Requests;
using VeryGoodNewsPortal.WebApi.Models.Responses;

namespace VeryGoodNewsPortal.WebApi.Mappers
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<AuthenticateRequest, LoginDto>();
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
            CreateMap<JwtAuthDto, AuthenticateResponse>();
            

        }
    }
}
