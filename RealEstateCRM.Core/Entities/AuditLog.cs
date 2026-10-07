using System;

namespace RealEstateCRM.Core.Entities
{
    public class AuditLog : BaseEntity
    {
        public string EntityName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // Create, Update, Delete
        public string UserId { get; set; } = "System";
        public string OldValues { get; set; } = string.Empty;
        public string NewValues { get; set; } = string.Empty;
    }
}