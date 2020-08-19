using System.Collections.Generic;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Persistence.Models;

namespace Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Contracts
{
    public interface IRepository<TEntity> : IRepositoryRead<TEntity> where TEntity : class, IIdentitifiedEntity
    {
        Task CreateAsync(TEntity entity, string createdBy = null);
        Task CreateAsync(TEntity[] entities, string createdBy = null);
        void Update(TEntity entity, string modifiedBy = null);
        void MarkAs(TEntity entity, string modifiedBy = null);
        Task DeleteAsync(object id);
        void DeleteList(List<TEntity> id);
        void Delete(TEntity entity);
        Task<int> SaveAsync();
        Task SetActivationAsync(object id, bool isActive);
    }
}
