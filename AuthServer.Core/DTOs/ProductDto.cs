﻿namespace AuthServer.Core.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string UserId { get; set; } = null!;
    }
}
