namespace findox.api.Controllers
{
    using Findox.Application.Dto.User;
    using Findox.Application.Services.Authenticate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public TokenController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService ?? throw new ArgumentNullException(nameof(authenticateService));
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            var response = await _authenticateService.AuthenticateAsync(request);

            if (response is null)
                return Unauthorized();

            return Ok(response);
        }
    }
}