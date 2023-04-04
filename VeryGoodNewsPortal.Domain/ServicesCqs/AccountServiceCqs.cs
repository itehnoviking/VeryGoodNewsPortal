using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Cqs.Models.Commands.AccountCommands;
using VeryGoodNewsPortal.Cqs.Models.Queries.AccountQueries;

namespace VeryGoodNewsPortal.Domain.ServicesCqs
{
    public class AccountServiceCqs : IAccountServiceCqs
    {
        private readonly ILogger<AccountServiceCqs> _logger;
        private readonly  IMediator _mediator;
        private IConfiguration _configuration
            ;

        public AccountServiceCqs(ILogger<AccountServiceCqs> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<UserDto> GetUserByRefreshTokenAsync(string token)
        {
            try
            {
                var user = await _mediator.Send(new GetUserByRefreshTokenQuery(token), new CancellationToken());

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            try
            {
                var upperEmail = email.ToUpperInvariant();

                var user = await _mediator.Send(new GetUserByEmailQuery(upperEmail),
                    new CancellationToken());

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> CheckPasswordByEmailAsync(string email, string password)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);

                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    var enteredPasswordHash = GetPasswordHash(password, _configuration["ApplicationVariables:Salt"]);

                    if (enteredPasswordHash.Equals($@"{user.PasswordHash}"))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> CreateUserAsync (RegisterDto dto, string roleName)
        {
            //todo added validation dto

            var passwordHash = GetPasswordHash(dto.Password, _configuration["ApplicationVariables:Salt"]);
            var normalizedEmail = dto.Email.ToUpperInvariant();

            var command = new RegisterUserCommand(dto.Email, normalizedEmail, dto.Name, passwordHash);

            await _mediator.Send(command, new CancellationToken());

            return await SetRoleUserAsync(command.Id, roleName);
        }

        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            try
            {
                if (!await CheckPasswordByEmailAsync(email, oldPassword))
                {
                    throw new ArgumentException("Incorrect email or current password");
                }

                var user = await GetUserByEmailAsync(email);

                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                var newPasswordHash = GetPasswordHash(newPassword, _configuration["ApplicationVariables:Salt"]);
                var command = new ChangePasswordCommand(user.Id, newPasswordHash);

                return await _mediator.Send(command, new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private string GetPasswordHash(string password, string salt)
        {
            try
            {
                var sha1 = new SHA1CryptoServiceProvider();

                var sha1Data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{salt}_{password}"));
                var hashedPassword = Encoding.UTF8.GetString(sha1Data);

                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<bool> SetRoleUserAsync(Guid userId, string roleName)
        {
            try
            {
                var roleId = await _mediator.Send(new GetRoleIdByNameQuery(roleName),
                    new CancellationToken());

                var command = new CreateUserRoleCommand()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = roleId
                };
                return await _mediator.Send(command, new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        
    }

    
}
