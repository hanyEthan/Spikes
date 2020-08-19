using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Persistence.Models;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Contracts;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Handlers
{
    public class DbRepositoryRead<TEntity> : DbRepositoryBase<TEntity>, IRepositoryRead<TEntity> where TEntity : class, IIdentitifiedEntity
    {
        #region cst.

        public DbRepositoryRead(DbContext context) : base(context)
        {
        }

        #endregion
        #region publics.

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool detached = false)
        {
            return await GetQueryable(detached, filter: null, orderBy, includeProperties, skip, take).ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool detached = false)
        {
            return await GetQueryable(detached, filter, orderBy, includeProperties, skip, take).ToListAsync();
        }
        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "", bool detached = false)
        {
            return await GetQueryable(detached, filter, null, includeProperties).SingleOrDefaultAsync();
        }
        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool detached = false)
        {
            return await GetQueryable(detached, filter, orderBy, includeProperties).FirstOrDefaultAsync();
        }
        public virtual async Task<TEntity> GetByIdAsync(object id, bool detached = false, string includeProperties = null)
        {
            return await GetQueryable(detached, x => x.Id == id, null, includeProperties).SingleOrDefaultAsync();
        }
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable(false, filter).CountAsync();
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable(false, filter).AnyAsync();
        }

        #endregion
        #region helpers.

        protected virtual IQueryable<TEntity> GetQueryable(bool detached, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = context.Set<TEntity>();

            // detaching
            if (detached)
            {
                this.context.ChangeTracker.LazyLoadingEnabled = false;
                //this.context.Configuration.ProxyCreationEnabled = false;
                query = detached ? query.AsNoTracking() : query;
            }

            if (filter != null) query = query.Where(filter);   // filters

            // includes ...
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // ordering ...
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // paging

            //The resulting SQL statement is parameterized and can be cached efficiently by SQL Server
            //Because we use a lambda version of the Skip() and Take() extensions 
            if (skip.HasValue && skip != 0) query = query.Skip(skip.Value);
            if (take.HasValue && take != 0) query = query.Take(take.Value);

            // ...
            return query;
        }
        protected virtual IQueryable<TEntity> GetQueryable(bool detached, IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            includeProperties = includeProperties ?? string.Empty;
            query = query ?? context.Set<TEntity>();

            // detaching
            if (detached)
            {
                this.context.ChangeTracker.LazyLoadingEnabled = false;
                //this.context.Configuration.ProxyCreationEnabled = false;
                query = detached ? query.AsNoTracking() : query;
            }

            // includes ...
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // ordering ...
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // paging

            //The resulting SQL statement is parameterized and can be cached efficiently by SQL Server
            //Because we use a lambda version of the Skip() and Take() extensions 
            if (skip.HasValue && skip != 0) query = query.Skip(skip.Value);
            if (take.HasValue && take != 0) query = query.Take(take.Value);

            // ...
            return query;
        }

        #endregion
    }
}
