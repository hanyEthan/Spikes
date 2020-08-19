using System.Collections.Generic;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Handlers
{
    public interface IRepository<TEntity> : IRepositoryRead<TEntity> where TEntity : class, IEntity
    {
        Task CreateAsync( TEntity entity , string createdBy = null );
        Task CreateAsync(TEntity[] entities, string createdBy = null);
        void Update( TEntity entity , string modifiedBy = null );
        void MarkAs(TEntity entity, string modifiedBy = null);
        Task DeleteAsync( object id );
        void DeleteList(List<TEntity> id);
        void Delete( TEntity entity );
        Task<int> SaveAsync();
        Task SetActivationAsync(object id, bool isActive);
    }
}
