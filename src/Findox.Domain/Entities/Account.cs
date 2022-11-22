using System.Text.Json.Serialization;

namespace Findox.Domain.Entities
{
    public sealed class Account
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastLogin { get; set; }
        public IEnumerable<Role> Roles { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public Account()
        {
            Roles = Array.Empty<Role>();
        }
    }
}