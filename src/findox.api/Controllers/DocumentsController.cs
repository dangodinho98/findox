namespace findox.api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;

        public DocumentsController(ILogger<DocumentsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Upload")]
        public void Upload()
        {

        }

        [HttpPost("Download")]
        public void Download()
        {

        }
        
        [HttpPost("grant-access")]
        public void GrantAccess()
        {

        }
        
        [HttpPost("revoke-access")]
        public void RevokeAccess()
        {

        }
    }
}