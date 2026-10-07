using System;

namespace RealEstateCRM.Core.DTOs
{
    public class CreateDealDto
    {
        public int PropertyId { get; set; }
        public int LeadId { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Commission { get; set; }
    }

    public class DealDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int LeadId { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Commission { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ContractDate { get; set; }
    }
}