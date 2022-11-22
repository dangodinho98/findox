using System.Security.Claims;
using Findox.Application.Services.Account;
using Findox.Infra.Authentication;
using Findox.Shared;
using Microsoft.Extensions.Options;

namespace Findox.Api.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IAccountService accountService, ITokenManager tokenManager)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = tokenManager.ValidateJwtToken(token);
        if (userId != null)
        {
            var user = await accountService.GetByIdAsync(userId.Value);
            // attach user to context on successful jwt validation
            context.Items["User"] = user;
            
            // Identity Principal
            var claims = new[]
            {
                new Claim("name", user.Username),
                new Claim(ClaimTypes.Role, Constants.Roles.Admin),
            };
            var identity = new ClaimsIdentity(claims, "basic");
            context.User = new ClaimsPrincipal(identity);
        }

        await _next(context);
    }
}