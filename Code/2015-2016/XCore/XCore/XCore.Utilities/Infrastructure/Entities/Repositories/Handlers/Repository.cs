using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using XCore.Utilities.Infrastructure.Entities.Repositories.Models;

namespace XCore.Utilities.Infrastructure.Entities.Repositories.Handlers
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

        public virtual void Create( TEntity entity , string createdBy = null )
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedBy = createdBy;

            context.Set<TEntity>().Add( entity );
        }
        public virtual void Update( TEntity entity , string modifiedBy = null )
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;

            // context.Set<TEntity>().Attach( entity );
            context.Entry( entity ).State = EntityState.Modified;
        }
        public virtual void MarkAs(TEntity entity, string modifiedBy = null)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;

            // context.Set<TEntity>().Attach( entity );
            context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete( object id )
        {
            TEntity entity = context.Set<TEntity>().Find( id );
            Delete( entity );
        }
        public virtual void Delete( TEntity entity )
        {
            var dbSet = context.Set<TEntity>();
            if ( context.Entry( entity ).State == EntityState.Detached ) dbSet.Attach( entity );

            dbSet.Remove( entity );
        }
        public virtual void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch ( DbEntityValidationException e )
            {
                ThrowEnhancedValidationException( e );
            }
        }
        public virtual Task SaveAsync()
        {
            try
            {
                return context.SaveChangesAsync();
            }
            catch ( DbEntityValidationException e )
            {
                ThrowEnhancedValidationException( e );
            }

            return Task.FromResult( 0 );
        }

        public void SetActivation(object id, bool isActive)
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            entity.IsActive = isActive;

            Update(entity);
        }
        #endregion
        #region helpers.

        protected virtual void ThrowEnhancedValidationException( DbEntityValidationException e )
        {
            var errorMessages = e.EntityValidationErrors
                                 .SelectMany( x => x.ValidationErrors )
                                 .Select( x => x.ErrorMessage );

            var fullErrorMessage = string.Join( "; " , errorMessages );
            var exceptionMessage = string.Concat( e.Message , " The validation errors are: " , fullErrorMessage );
            throw new DbEntityValidationException( exceptionMessage , e.EntityValidationErrors );
        }

        #endregion
    }

    #endregion
}
