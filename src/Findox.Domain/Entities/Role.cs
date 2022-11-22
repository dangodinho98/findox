namespace Findox.Domain.Entities
{
    using System.Text.Json.Serialization;

    public class Role
    {
        public Role()
        {
        }

        public Role(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
