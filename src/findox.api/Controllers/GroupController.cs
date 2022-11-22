using Findox.Shared;
using Microsoft.AspNetCore.Http.Extensions;

namespace findox.api.Controllers
{
    using Findox.Application.Dto.Group;
    using Findox.Application.Services.Group;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = Constants.Roles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupDto groupDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState) ;
            }

            var groupId = await _groupService.CreateAsync(groupDto);
            var group = await _groupService.GetByIdAsync(groupId);
            return Created(new Uri(Request.GetEncodedUrl()+ "/" + groupId), group);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] GroupDto groupDto, int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState) ;
            }

            await _groupService.UpdateAsync(id, groupDto);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == default)
                return NotFound();

            await _groupService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromBody] int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState) ;
            }

            var group = await _groupService.GetByIdAsync(id);
            if (group is null)
                return NotFound();
            
            return Ok(group);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            return Ok(await _groupService.GetAsync());
        }
    }
}