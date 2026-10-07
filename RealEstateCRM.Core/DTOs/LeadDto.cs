using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.DTOs;

public class LeadDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PreferredLocation { get; set; }
    public decimal EstimatedBudget { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateLeadDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PreferredLocation { get; set; }
    public decimal EstimatedBudget { get; set; }
}