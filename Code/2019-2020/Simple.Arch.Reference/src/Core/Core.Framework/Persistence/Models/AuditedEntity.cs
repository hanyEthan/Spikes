using System;
using System.Collections.Generic;
using System.Text;

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{

    public abstract class AuditedEntity : CreationAuditedEntity, IAudited
    {
        /// <summary>
        /// Last modification date of this entity.
        /// </summary>
        public virtual DateTime? LastModificationDateTimeUtc { get; set; }

        /// <summary>
        /// Last modifier user of this entity.
        /// </summary>
        public virtual string LastModifiedByUserId { get; set; }
    }
}
