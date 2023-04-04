using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands
{

    public class RegisterUserCommand : IRequest<bool>
    {
        public RegisterUserCommand(string email, string normalizedEmail, string name, string passwordHashdHash)
        {
                Id = Guid.NewGuid();
                Email = email;
                NormalizedEmail = normalizedEmail;
                PasswordHash = passwordHashdHash;
                Name = name;
                RegistrationDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? PasswordHash { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

}
