namespace RealEstateCRM.Core.Enums;
public enum LeadStatus
{
    New = 1,          // Newly acquired lead
    Contacted = 2,    // Initial contact established
    Qualified = 3,    // High potential / Negotiations active
    Converted = 4,    // Deal closed / Converted to customer
    Lost = 5          // Not interested / Closed lead
}
