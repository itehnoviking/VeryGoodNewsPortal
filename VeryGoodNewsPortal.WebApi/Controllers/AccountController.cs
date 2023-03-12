using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeryGoodNewsPortal.Core.DTOs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesCqs;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesWebApi;
using VeryGoodNewsPortal.WebApi.Models.Requests;
using VeryGoodNewsPortal.WebApi.Models.Responses;

namespace VeryGoodNewsPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountServiceCqs _accountServiceCqs;

        public AccountController(IMapper mapper, 
            ITokenService tokenService, 
            ILogger<AccountController> logger, IAccountServiceCqs accountServiceCqs)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
            _accountServiceCqs = accountServiceCqs;
        }

        [HttpPost("auth"), AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateRequest request)
        {
            try
            {
                var dto = _mapper.Map<LoginDto>(request);
                var response = await _tokenService.GetTokenAsync(dto, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }


                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("refresh-token"), AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refresh-token"];

                var response = await _tokenService.RefreshTokenAsync(refreshToken, GetIpAddress());

                if (response == null)
                {
                    return BadRequest(new ResponseMessage { Message = "Invalid token" });
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(_mapper.Map<AuthenticateResponse>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest request)
        {
            try
            {
                var token = request.Token ?? Request.Cookies["refresh-token"];

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { message = "Token is required" });
                }

                //var response = _tokenService.RevokeToken(token, GetIpAddress());
                await _tokenService.RevokeTokenAsync(token, GetIpAddress());

                return Ok(new { message = "Token is revoked" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-forwarded-For"))
            {
                return Request.Headers["X-forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }

        private void SetTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(2)
            };

            Response.Cookies.Append("refresh-token", refreshToken, cookieOptions);
        }

    }
}
