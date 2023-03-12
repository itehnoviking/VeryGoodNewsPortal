using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using VeryGoodNewsPortal.Core.Interfaces;
using VeryGoodNewsPortal.Core.Interfaces.InterfacesWebApi;

namespace WebApiFirstMvcApp.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }


        public async Task Invoke(HttpContext context, IJwtService jwtService, IAccountService accountService)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            var userId = jwtService.ValidateJwtToken(token);

            if (userId != null)
            {
                context.Items["User"] = accountService.GetUserByIdAsync(userId.Value);
            }

            await _next(context);
        }
        

        
    }
}
