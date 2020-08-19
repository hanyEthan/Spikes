

namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface IDeletionAudited : IHasDeletionTime
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
      string DeletedByUserId { get; set; }
    }
}
