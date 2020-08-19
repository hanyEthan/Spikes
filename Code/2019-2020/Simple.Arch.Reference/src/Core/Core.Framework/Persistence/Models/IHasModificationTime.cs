using System;


namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? LastModificationDateTimeUtc { get; set; }
    }
}
