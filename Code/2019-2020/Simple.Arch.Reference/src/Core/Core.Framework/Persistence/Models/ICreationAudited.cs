namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface ICreationAudited : IHasCreationTime
    {
        /// <summary>
        /// Id of the creator user of this entity.
        /// </summary>
       string CreatedByUserId { get; set; }
    }
}
