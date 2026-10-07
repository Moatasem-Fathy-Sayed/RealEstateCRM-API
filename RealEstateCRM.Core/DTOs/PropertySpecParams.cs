using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.DTOs
{
    /// <summary>
    /// Parameters class for encapsulating query filtering, sorting, and pagination options for properties.
    /// </summary>
    public class PropertySpecParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        // Pagination Properties
        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Filtering Properties
        public PropertyType? Type { get; set; }
        public PropertyStatus? Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? City { get; set; }
        public string? Search { get; set; } // Matches Title or Description

        // Sorting Option (e.g., "priceAsc", "priceDesc", "dateDesc")
        public string? Sort { get; set; }
    }
}