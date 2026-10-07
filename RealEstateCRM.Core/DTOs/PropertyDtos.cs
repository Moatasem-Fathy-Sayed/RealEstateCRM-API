using System;
using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.DTOs
{
    public class CreatePropertyDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double AreaSize { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class PropertyDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double AreaSize { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
    }
}