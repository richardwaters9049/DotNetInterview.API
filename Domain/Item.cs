namespace DotNetInterview.API.Domain
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public List<Variation> Variations { get; set; } = new();
    }
}