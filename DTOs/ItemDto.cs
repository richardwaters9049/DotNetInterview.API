namespace DotNetInterview.API.DTOs
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public List<VariationDto> Variations { get; set; } = new();
    }
}