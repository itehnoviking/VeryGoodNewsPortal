using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Models;

namespace VeryGoodNewsPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISourceService _sourceService;
        private readonly IRssService _rssService;
        private readonly IHtmlParserService _htmlParserService;
        private readonly IRoleService _roleService;


        public AccountController(IMapper mapper, IAccountService accountService, ILogger<AccountController> logger, IConfiguration configuration, ISourceService sourceService, IRssService rssService, IHtmlParserService htmlParserService, IRoleService roleService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _logger = logger;
            _configuration = configuration;
            _sourceService = sourceService;
            _rssService = rssService;
            _htmlParserService = htmlParserService;
            _roleService = roleService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                var model = new AccountLoginModel()
                {
                    ReturnUrl = returnUrl
                };
                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            if (await _accountService.CheckPasswordAsync(model.Email, model.Password))
            {
                var userId = (await _accountService.GetUserIdByEmailAsync(model.Email)).GetValueOrDefault();

                var roleClaims =
                    (await _accountService.GetRolesAsync(userId))
                    .Select(rn => new Claim(ClaimTypes.Role, rn));

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, model.Email)
                };

                claims.AddRange(roleClaims);

                var claimsIdentity = new ClaimsIdentity(claims, authenticationType: "Cookie");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Redirect(model.ReturnUrl ?? "/");
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                var model = new AccountRegisterModel()
                {
                    ReturnUrl = returnUrl
                };

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _accountService.CheckUserWithThatEmailIsExistAsync(model.Email))
                {
                    var userId = await _accountService.CreateUserAsync(model.Email);
                    await _accountService.SetRoleAsync(userId, "User");
                    await _accountService.SetPasswordAsync(userId, model.Password);

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.Role, "User")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, authenticationType: "Cookie");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Redirect(model.ReturnUrl ?? "/");
                }
                else
                {
                    ModelState.TryAddModelError("UserAlreadyExist", "User is already exist");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Route("access-denied")]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
