using System;
using RealEstateCRM.Core.Enums;

namespace RealEstateCRM.Core.Entities
{
    public class Appointment : BaseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
        public int LeadId { get; set; }
        public Lead Lead { get; set; } = null!;
        public DateTime AppointmentDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    }
}