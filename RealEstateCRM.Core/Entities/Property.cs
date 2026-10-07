using System;
using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.Entities
{
    /// <summary>
    /// Represents the Property domain entity in the CRM system.
    /// Inherits from BaseEntity to fulfill the Generic Repository constraint (where T : BaseEntity)
    /// and automatically inherit shared primary keys and metadata like Id and CreatedAt.
    /// </summary>
    public class Property : BaseEntity
    {
        // Property title/name (e.g., "Luxury Apartment in Corniche El Nile")
        public string Title { get; set; } = string.Empty;

        // Detailed description of the real estate listing
        public string Description { get; set; } = string.Empty;

        // Financial price of the property
        public decimal Price { get; set; }

        // Total property area size measured in square meters
        public double AreaSize { get; set; }

        // Number of bedrooms
        public int Bedrooms { get; set; }

        // Number of bathrooms
        public int Bathrooms { get; set; }

        // Full street address of the property
        public string Address { get; set; } = string.Empty;

        // City location (e.g., "Beni Suef", "Cairo")
        public string City { get; set; } = string.Empty;

        // Enumeration specifying property type (e.g., Apartment, Villa, Duplex, Office, Land)
        public PropertyType Type { get; set; }

        // Enumeration specifying current listing status (e.g., Available, UnderContract, Sold, Rented)
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;

        // Foreign Key referencing the assigned ApplicationUser (Agent)
        public string? AgentId { get; set; }

        // Navigation Property pointing to the assigned Agent account
        public ApplicationUser? Agent { get; set; }
        public string? ImageUrl { get; set; }
    }
}