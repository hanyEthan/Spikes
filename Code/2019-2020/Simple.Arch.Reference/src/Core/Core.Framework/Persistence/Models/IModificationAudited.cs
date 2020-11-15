namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface IModificationAudited : IHasModificationTime
    {
        /// <summary>
        /// Last modifier user for this entity.
        /// </summary>
        string LastModifiedByUserId { get; set; }
    }
}
