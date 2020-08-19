using System;

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface IHasDeletionTime : ISoftDelete
    {
        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        DateTime? DeletionDateTimeUtc { get; set; }
    }
}
