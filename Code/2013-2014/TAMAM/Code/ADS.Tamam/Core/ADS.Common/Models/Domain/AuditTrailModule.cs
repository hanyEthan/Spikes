using System;

namespace ADS.Common.Models.Domain
{
    public class AuditTrailModule
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }

        public int Code { get; set; }
    }
}
