using System;

namespace ADS.Common.Models.Domain
{
    public class AuditTrailAction
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }

        public int Code { get; set; }
    }
}
