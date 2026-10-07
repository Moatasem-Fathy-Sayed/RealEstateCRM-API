using System;

namespace RealEstateCRM.Core.DTOs
{
    public class CreateAppointmentDto
    {
        public int PropertyId { get; set; }
        public int LeadId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int LeadId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}