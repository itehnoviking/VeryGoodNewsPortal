using VeryGoodNewsPortal.Core.DTOs;

namespace VeryGoodNewsPortal.WebApi.Models.Responses
{
    public class RegisterResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<UserRoleDto> UserRoles { get; set; }
    }
}
