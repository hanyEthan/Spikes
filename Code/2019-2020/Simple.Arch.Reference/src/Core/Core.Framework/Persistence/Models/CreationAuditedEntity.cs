using System;

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public abstract class CreationAuditedEntity : ICreationAudited
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public virtual DateTime CreationDateTimeUtc { get; set; }

        /// <summary>
        /// Creator of this entity.
        /// </summary>
        public virtual string CreatedByUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreationAuditedEntity()
        {
            CreationDateTimeUtc = DateTime.UtcNow;
        }
    }
}
