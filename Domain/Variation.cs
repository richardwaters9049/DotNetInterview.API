namespace DotNetInterview.API.Domain
{
    public class Variation
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public required string Size { get; set; }
        public int Quantity { get; set; }

        // Add this navigation property
        public Item? Item { get; set; }
    }
}