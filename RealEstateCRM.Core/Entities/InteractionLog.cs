using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.Entities;

public class InteractionLog : BaseEntity
{
    public int LeadId { get; set; }
    public InteractionType Type { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime? NextFollowUpDate { get; set; }

    // Navigation Property
    public Lead Lead { get; set; } = null!;
}