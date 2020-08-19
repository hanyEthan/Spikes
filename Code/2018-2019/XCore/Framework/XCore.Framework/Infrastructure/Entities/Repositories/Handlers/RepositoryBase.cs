using System;
using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Handlers
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        #region props.

        protected readonly DbContext context;

        private bool disposed = false;

        #endregion
        #region cst.

        public RepositoryBase( DbContext context )
        {
            this.context = context;
        }

        #endregion
        #region publics.

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion
        #region helpers.

        protected virtual void Dispose( bool disposing )
        {
            if ( !this.disposed )
            {
                if ( disposing )
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        #endregion
    }
}
