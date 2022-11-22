using System.ComponentModel.DataAnnotations;

namespace Findox.Application.Dto.User
{
    public class AuthenticateRequest
    {
        [Required]
        public string? Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
