using AutoMapper;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands;
using VeryGoodNewsPortal.Data.Entities;
using VeryGoodNewsPortal.WebApi.Models.Requests;
using VeryGoodNewsPortal.WebApi.Models.Responses;

namespace VeryGoodNewsPortal.WebApi.Mappers
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AuthenticateRequest, LoginDto>();
            CreateMap<RegisterRequest, RegisterDto>();

            CreateMap<RegisterUserCommand, User>();

            CreateMap<CreateUserRoleCommand, UserRole>();

            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
            CreateMap<JwtAuthDto, AuthenticateResponse>();
            

        }
    }
}
