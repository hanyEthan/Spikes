namespace Mcs.Invoicing.Core.Framework.Persistence.Models
{
    public interface IIdentitifiedEntity : IFullAudited
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        object Id { get; set; }
    }
    public interface IIdentitifiedEntity<TId> : IIdentitifiedEntity
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        new TId Id { get; set; }
    }
}
