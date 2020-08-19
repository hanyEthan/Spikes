using System;

namespace ADS.Tamam.Services.DataAcquisition.Models
{
    public class PullMechanismModel
    {
        public string Id { get; set; }
        public string PersonId { get; set; }
        public DateTime EventDate { get; set; }
        public string TerminalId { get; set; }
        public int EventType { get; set; }
        public string Logsource { get; set; }
        public string LogOrgin { get; set; }
        public string ConsiderLogForAttendance { get; set; }
        public string LocationName { get; set; }
    }
}
