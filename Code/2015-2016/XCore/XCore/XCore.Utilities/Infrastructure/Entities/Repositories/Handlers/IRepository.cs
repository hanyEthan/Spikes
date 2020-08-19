using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Entities.Repositories.Models;

namespace XCore.Utilities.Infrastructure.Entities.Repositories.Handlers
{
    public interface IRepository<TEntity> : IRepositoryRead<TEntity> where TEntity : class, IEntity
    {
        void Create( TEntity entity , string createdBy = null );
        void Update( TEntity entity , string modifiedBy = null );
        void MarkAs(TEntity entity, string modifiedBy = null);
        void Delete( object id );
        void Delete( TEntity entity );
        void Save();
        Task SaveAsync();
        void SetActivation(object id, bool isActive);
    }
}
