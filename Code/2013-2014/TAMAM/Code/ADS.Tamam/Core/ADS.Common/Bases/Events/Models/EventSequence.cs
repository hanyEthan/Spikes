using ADS.Common.Bases.Events.Contracts;
using System.Collections.Generic;
using System;

namespace ADS.Common.Bases.Events.Models
{
    public class EventSequence : IEventCell
    {
        public Guid Id { get; set; }
        public string ContentType { get; set; }
        public string TargetId { get; set; }
        public string TargetType { get; set; }
        public List<IEventCell> NestedEvents { get; set; }

        public bool Process()
        {
            throw new NotImplementedException();
        }
    }
}
