using System;
using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class ShiftPolicy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public float? InGraceTime { get; set; }
        public float? OutGraceTime { get; set; }
        public float? MinimumHours { get; set; }
        public float? MaximumHours { get; set; }

        public IList<Shift> Shifts { get; set; } 
    }
}
