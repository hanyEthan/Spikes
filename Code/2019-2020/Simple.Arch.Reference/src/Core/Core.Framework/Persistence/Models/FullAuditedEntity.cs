using System;

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public abstract class FullAuditedEntity : AuditedEntity, IFullAudited
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        public virtual string DeletedByUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        public virtual DateTime? DeletionDateTimeUtc { get; set; }
    }
}
