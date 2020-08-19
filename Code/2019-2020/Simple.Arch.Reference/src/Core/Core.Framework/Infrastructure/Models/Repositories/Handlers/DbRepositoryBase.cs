using System;
using Mcs.Invoicing.Core.Framework.Persistence.Models;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Handlers
{
    public abstract class DbRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IIdentitifiedEntity
    {
        #region props.

        protected readonly DbContext context;

        private bool disposed = false;

        #endregion
        #region cst.

        public DbRepositoryBase(DbContext context)
        {
            this.context = context;
        }

        #endregion
        #region publics.

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        #region helpers.

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        #endregion
    }
}
