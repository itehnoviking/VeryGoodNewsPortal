using AutoMapper;
using FirstMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.Data;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesWebApi;
using VeryGoodNewsPortal.Data.Entities;

namespace VeryGoodNewsPortal.Domain.ServicesWebApi
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ILogger<TokenService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IAccountServiceCqs _accountServiceCqs;
        private readonly ITokenServiceCqs _tokenServiceCqs;

        public TokenService(IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IAccountService accountService,
            ILogger<TokenService> logger,
            IJwtService jwtService,
            IMapper mapper,
            IAccountServiceCqs accountServiceCqs, IMediator mediator, ITokenServiceCqs tokenServiceCqs)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _logger = logger;
            _jwtService = jwtService;
            _mapper = mapper;
            _accountServiceCqs = accountServiceCqs;
            _tokenServiceCqs = tokenServiceCqs;
        }

        public async Task<JwtAuthDto> GetTokenAsync(LoginDto request, string ipAddress)
        {
            var user = await _accountServiceCqs.GetUserByEmailAsync(request.Login);

            if (!await _accountServiceCqs.CheckPasswordByEmailAsync(request.Login, request.Password))
            {
                _logger.LogWarning("Trying to get jwt-token with incorrect password");
                return null;
            }

            var jwtToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken(ipAddress);

            //refreshToken.Id = Guid.NewGuid();
            refreshToken.UserId = user.Id;

            await _unitOfWork.RefreshTokens.AddAsync(_mapper.Map<RefreshToken>(refreshToken));

            await _unitOfWork.Commit();

            return new JwtAuthDto(user, jwtToken, refreshToken.Token);
        }

        public async Task<JwtAuthDto> RefreshTokenAsync(string? token, string ipAddress)
        {
            var user = await _accountServiceCqs.GetUserByRefreshTokenAsync(token);

            var refreshToken = await _tokenServiceCqs.GetRefreshTokenAsync(token);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                throw new ArgumentException("Invalid token", "token");
            }

            if (!refreshToken.IsRevoked)
            {
                await RevokeDescendantRefreshToken(refreshToken, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
            }

            var refreshTokenDto = await RotateRefreshToken(refreshToken, ipAddress);

            refreshTokenDto.UserId = user.Id;

            await _unitOfWork.RefreshTokens.AddAsync(_mapper.Map<RefreshToken>(refreshTokenDto));
            await _unitOfWork.Commit();

            await RemoveOldRefreshTokens(user);

            var jwtToken = _jwtService.GenerateJwtToken(user);

            return new JwtAuthDto(user, jwtToken, refreshTokenDto.Token);
        }

        public async Task RevokeTokenAsync(string token, string ipAddress)
        {
            var refreshToken = await _tokenServiceCqs.GetRefreshTokenAsync(token);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                throw new ArgumentException("Invalid token", "token");
            }

            await RevokeRefreshToken(refreshToken, ipAddress, $"Revoke without replacement: {token}");
        }


        private async Task RevokeDescendantRefreshToken(RefreshTokenDto token, string ipAddress, string reason)
        {
            if (!string.IsNullOrEmpty(token.ReplaceByToken))
            {
                var childToken = await _tokenServiceCqs.GetChildTokenAsync(token);

                if (childToken.IsActive)
                {
                    await RevokeRefreshToken(childToken, ipAddress, reason);
                }

                else
                {
                    await RevokeDescendantRefreshToken(childToken, ipAddress, reason);
                }
            }
        }

        private async Task RevokeRefreshToken(RefreshTokenDto token, string ipAddress, string reason = null, string replaceByToken = null)
        {
            await _unitOfWork.RefreshTokens.PatchAsync(token.Id, new List<PatchModel>()
            {
                new PatchModel()
                {
                    PropertyName = "Revoked",
                    PropertyValue = DateTime.UtcNow
                },
                new PatchModel()
                {
                    PropertyName = "RevokedByIp",
                    PropertyValue = ipAddress
                },
                new PatchModel()
                {
                    PropertyName = "ReasonOfRevoke",
                    PropertyValue = reason
                },
                new PatchModel()
                {
                    PropertyName = "ReplaceByToken",
                    PropertyValue = replaceByToken
                }
            });

            await _unitOfWork.Commit();

        }

        private async Task<RefreshTokenDto> RotateRefreshToken(RefreshTokenDto token, string ipAddress)
        {
            var newRefreshToken = _jwtService.GenerateRefreshToken(ipAddress);

            await RevokeRefreshToken(token, ipAddress, "Replace by new token", newRefreshToken.Token);

            return newRefreshToken;
        }

        private async Task RemoveOldRefreshTokens(UserDto userDto)
        {

            await _unitOfWork.RefreshTokens.RemoveRange(token => !token.IsActive && token.UserId.Equals(userDto.Id));
            await _unitOfWork.Commit();
        }
    }
}
