using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.Entities;

public class Lead : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PreferredLocation { get; set; }
    public decimal EstimatedBudget { get; set; }

    public LeadStatus Status { get; set; } = LeadStatus.New;

    // Navigation Properties
    public List<InteractionLog> Interactions { get; set; } = new();
}