namespace Findox.Application.Dto.Account
{
    using System.ComponentModel.DataAnnotations;

    public class CreateAccountRequest
    {
        [Required][MaxLength(50)] public string Username { get; set; }
        [Required][MaxLength(50)] public string Password { get; set; }
        [MaxLength(255)][EmailAddress] public string Email { get; set; }

        public string[] RoleNames { get; set; }
    }
}