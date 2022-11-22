namespace findox.api.Controllers
{
    using Findox.Application.Dto.Account;
    using Findox.Application.Services.Account;
    using Findox.Shared;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = Constants.Roles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _accountService.CreateAsync(request);
            return Created(new Uri($"{Request.GetEncodedUrl()}/{account!.UserId}"), account);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateAccountRequest request, int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState) ;
            }

            await _accountService.UpdateAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default)
                return NotFound();

            await _accountService.DeleteAsync(id);
            return NoContent();
        }
    }
}