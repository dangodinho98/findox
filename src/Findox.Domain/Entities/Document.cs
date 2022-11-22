namespace Findox.Domain.Entities
{
    public sealed class Document
    {
        public DateTime PostedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
