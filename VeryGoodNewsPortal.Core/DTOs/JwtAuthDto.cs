using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryGoodNewsPortal.Core.DTOs
{
    public class JwtAuthDto
    {
        public JwtAuthDto(UserDto user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Email = user.Email;
            RoleNames = user.RoleNames;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string[] RoleNames { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
