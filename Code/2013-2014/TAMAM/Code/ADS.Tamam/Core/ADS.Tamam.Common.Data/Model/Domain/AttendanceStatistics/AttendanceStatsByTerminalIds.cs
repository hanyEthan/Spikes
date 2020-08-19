using System;
using System.Collections.Generic;
using System.Data;
using ADS.Common.Contracts;

namespace ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics
{
    [Serializable]
    public class TerminalStatsComposite : IXSerializable
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public string AttendanceType { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string LocationName { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
