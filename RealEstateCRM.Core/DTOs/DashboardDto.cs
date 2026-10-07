using System.Collections.Generic;

namespace RealEstateCRM.Core.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalProperties { get; set; }
        public int TotalLeads { get; set; }
        public int TotalInteractions { get; set; }
        public decimal TotalPipelineBudget { get; set; }
        public Dictionary<string, int> LeadsByStatus { get; set; } = new();
        public List<RecentInteractionDto> RecentInteractions { get; set; } = new();
    }

    public class RecentInteractionDto
    {
        public int Id { get; set; }
        public string LeadName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public System.DateTime CreatedAt { get; set; }
    }
}