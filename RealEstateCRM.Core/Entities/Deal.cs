using System;
using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.Entities
{
    public class Deal : BaseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public int LeadId { get; set; }
        public Lead Lead { get; set; } = null!;
        public decimal SalePrice { get; set; }
        public decimal Commission { get; set; }
        public DealStatus Status { get; set; } = DealStatus.Pending;
        public DateTime ContractDate { get; set; } = DateTime.UtcNow;
    }
}