using System;

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public abstract class IdentitifiedEntity<TId> : FullAuditedEntity, IIdentitifiedEntity<TId>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public virtual TId Id { get; set; }
        object IIdentitifiedEntity.Id { get { return this.Id; } set { this.Id = (TId)Convert.ChangeType(value, typeof(TId)); } }

        /// <summary>
        /// Integration Id of the entity.
        /// </summary>
        public virtual string Code { get; set; } = Guid.NewGuid().ToString();
    }
}
