using AutoMapper;
using System.Linq;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.WebApi.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleNames,
                    opt => opt.MapFrom(src => src.UserRoles.Select(role => role.Role.Name)));
        }
    }
}
