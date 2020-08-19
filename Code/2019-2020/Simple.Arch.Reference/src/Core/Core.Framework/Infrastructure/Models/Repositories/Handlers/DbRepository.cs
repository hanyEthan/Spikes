using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Persistence.Models;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Handlers
{
    public class DbRepository<TEntity> : DbRepositoryRead<TEntity>, IRepository<TEntity> where TEntity : class, IIdentitifiedEntity
    {
        #region cst.

        public DbRepository(DbContext context) : base(context)
        {
        }

        #endregion
        #region publics.

        public virtual async Task CreateAsync(TEntity entity, string createdBy = null)
        {
            entity.CreationDateTimeUtc = DateTime.UtcNow;
            entity.CreatedByUserId = createdBy;

            await context.Set<TEntity>().AddAsync(entity);
        }
        public virtual async Task CreateAsync(TEntity[] entities, string createdBy = null)
        {
            foreach (var entity in entities)
            {
                entity.CreationDateTimeUtc = DateTime.UtcNow;
                entity.CreatedByUserId = createdBy;
            }

            await context.Set<TEntity>().AddRangeAsync(entities);
        }
        public virtual async Task CreateAsync(List<TEntity> entities, string createdBy = null)
        {
            foreach (var entity in entities)
            {
                entity.CreationDateTimeUtc = DateTime.UtcNow;
                entity.CreatedByUserId = createdBy;
            }

            await context.Set<TEntity>().AddRangeAsync(entities);
        }
        public virtual void Update(TEntity entity, string modifiedBy = null)
        {
            entity.LastModificationDateTimeUtc = DateTime.UtcNow;
            entity.LastModifiedByUserId = modifiedBy;

            // context.Set<TEntity>().Attach( entity );
            context.Entry(entity).State = EntityState.Modified;
            // context.Set<TEntity>().Update(entity);
        }
        public virtual void MarkAs(TEntity entity, string modifiedBy = null)

        {
            entity.LastModificationDateTimeUtc = DateTime.UtcNow;
            entity.LastModifiedByUserId = modifiedBy;

            // context.Set<TEntity>().Attach( entity );
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task DeleteAsync(object id)
        {
            TEntity entity = await context.Set<TEntity>().FindAsync(id);
            Delete(entity);
        }
        public virtual void Delete(TEntity entity)
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached) dbSet.Attach(entity);

            dbSet.Remove(entity);
        }
        public virtual void DeleteList(List<TEntity> entities)
        {
            context.RemoveRange(entities);
        }
        public virtual void Delete<TEntityOther>(TEntityOther entity) where TEntityOther : class
        {
            var dbSet = context.Set<TEntityOther>();
            if (context.Entry(entity).State == EntityState.Detached) dbSet.Attach(entity);

            dbSet.Remove(entity);
        }
        public virtual async Task<int> SaveAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task SetActivationAsync(object id, bool isActive)
        {
            TEntity entity = await context.Set<TEntity>().FindAsync(id);
            entity.IsDeleted = !isActive;
            entity.DeletionDateTimeUtc = entity.DeletionDateTimeUtc.HasValue && isActive == true
                                       ? null
                                       : !entity.DeletionDateTimeUtc.HasValue && isActive == false
                                       ? DateTime.UtcNow
                                       : entity.DeletionDateTimeUtc;

            Update(entity);
        }

        #endregion
    }
}
