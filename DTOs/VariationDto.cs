namespace DotNetInterview.API.DTOs
{
    public class VariationDto
    {
        public Guid Id { get; set; }
        public string Size { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}