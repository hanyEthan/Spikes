using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Handlers
{
    #region context / entity

    public class Repository<TEntity> : RepositoryRead<TEntity>, IRepository<TEntity> where TEntity : class, IEntity
    {
        #region cst.

        public Repository( DbContext context ) : base( context )
        {
        }

        #endregion
        #region publics.

        public virtual async Task CreateAsync( TEntity entity , string createdBy = null )
        {
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = createdBy;

            await context.Set<TEntity>().AddAsync( entity );
        }
        public virtual async Task CreateAsync(TEntity[] entities, string createdBy = null)
        {
            foreach (var entity in entities)
            {
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = createdBy;
            }

            await context.Set<TEntity>().AddRangeAsync(entities);
        }
        public virtual async Task CreateAsync(List<TEntity> entities, string createdBy = null)
        {
            foreach (var entity in entities)
            {
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = createdBy;
            }

            await context.Set<TEntity>().AddRangeAsync(entities);
        }
        public virtual void Update( TEntity entity , string modifiedBy = null )
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;

            // context.Set<TEntity>().Attach( entity );
            context.Entry( entity ).State = EntityState.Modified;
           // context.Set<TEntity>().Update(entity);
        }
        public virtual void MarkAs(TEntity entity, string modifiedBy = null)

        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;

            // context.Set<TEntity>().Attach( entity );
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task DeleteAsync( object id )
        {
            TEntity entity = await context.Set<TEntity>().FindAsync( id );
            Delete( entity );
        }
        public virtual void Delete( TEntity entity )
        {
            var dbSet = context.Set<TEntity>();
            if ( context.Entry( entity ).State == EntityState.Detached ) dbSet.Attach( entity );

            dbSet.Remove( entity );
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
            entity.IsActive = isActive;

            Update(entity);
        }

        #endregion
    }

    #endregion
}
