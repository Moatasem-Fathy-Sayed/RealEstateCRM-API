using System;
using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.DTOs
{
    public class InteractionLogDto
    {
        public int Id { get; set; }
        public int LeadId { get; set; }
        public string LeadName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Call, Meeting, Email, Note
        public string Notes { get; set; } = string.Empty;
        public DateTime InteractionDate { get; set; }
    }

    public class CreateInteractionLogDto
    {
        public int LeadId { get; set; }
        public InteractionType Type { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime? InteractionDate { get; set; }
    }
}