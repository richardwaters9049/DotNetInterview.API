﻿namespace DotNetInterview.API.Domain
{
    public class Variation
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string Size { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}