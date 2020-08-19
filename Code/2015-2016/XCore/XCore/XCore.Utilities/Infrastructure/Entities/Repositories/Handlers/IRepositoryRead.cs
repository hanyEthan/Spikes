using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using XCore.Utilities.Infrastructure.Entities.Repositories.Models;

namespace XCore.Utilities.Infrastructure.Entities.Repositories.Handlers
{
    public interface IRepositoryRead<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        IEnumerable<TEntity> GetAll( Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , int? skip = null , int? take = null , bool detached = false );
        IEnumerable<TEntity> Get( Expression<Func<TEntity , bool>> filter = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , int? skip = null , int? take = null , bool detached = false );
        TEntity GetOne( Expression<Func<TEntity , bool>> filter = null , string includeProperties = null , bool detached = false );
        TEntity GetFirst( Expression<Func<TEntity , bool>> filter = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , bool detached = false );
        TEntity GetById( object id , bool detached = false , string includeProperties = null );
        int Count( Expression<Func<TEntity , bool>> filter = null );
        bool Any( Expression<Func<TEntity , bool>> filter = null );

        Task<IEnumerable<TEntity>> GetAllAsync( Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , int? skip = null , int? take = null , bool detached = false );
        Task<IEnumerable<TEntity>> GetAsync( Expression<Func<TEntity , bool>> filter = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , int? skip = null , int? take = null , bool detached = false );
        Task<TEntity> GetOneAsync( Expression<Func<TEntity , bool>> filter = null , string includeProperties = null , bool detached = false );
        Task<TEntity> GetFirstAsync( Expression<Func<TEntity , bool>> filter = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , bool detached = false );
        Task<TEntity> GetByIdAsync( object id , bool detached = false );
        Task<int> CountAsync( Expression<Func<TEntity , bool>> filter = null );
        Task<bool> AnyAsync( Expression<Func<TEntity , bool>> filter = null );
    }
}
