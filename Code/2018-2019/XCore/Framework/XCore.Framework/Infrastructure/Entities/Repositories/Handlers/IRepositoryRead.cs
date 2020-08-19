using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Handlers
{
    public interface IRepositoryRead<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync( Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , int? skip = null , int? take = null , bool detached = false );
        Task<IEnumerable<TEntity>> GetAsync( Expression<Func<TEntity , bool>> filter = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , int? skip = null , int? take = null , bool detached = false );
        Task<TEntity> GetOneAsync( Expression<Func<TEntity , bool>> filter = null , string includeProperties = null , bool detached = false );
        Task<TEntity> GetFirstAsync( Expression<Func<TEntity , bool>> filter = null , Func<IQueryable<TEntity> , IOrderedQueryable<TEntity>> orderBy = null , string includeProperties = null , bool detached = false );
        Task<TEntity> GetByIdAsync( object id , bool detached = false , string includeProperties = null );
        Task<int> CountAsync( Expression<Func<TEntity , bool>> filter = null );
        Task<bool> AnyAsync( Expression<Func<TEntity , bool>> filter = null );
    }
}
