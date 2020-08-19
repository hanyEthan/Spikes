using System;

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreationDateTimeUtc { get; set; }
    }
}
