namespace BatteryAudit.Models
{
    using System;

    public class TabletReading
    {
        public int AcademyId { get; set; }
        public decimal BatteryLevel { get; set; }
        public string EmployeeId { get; set; }
        public string SerialNumber { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
