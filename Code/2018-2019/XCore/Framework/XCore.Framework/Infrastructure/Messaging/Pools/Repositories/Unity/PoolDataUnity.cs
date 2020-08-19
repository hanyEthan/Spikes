using System;
using XCore.Framework.Infrastructure.Messaging.Pools.Contracts;
using XCore.Framework.Infrastructure.Messaging.Pools.Repositories.Context;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Repositories.Unity
{
    public class PoolDataUnity<TRepo, TSettings> : IDisposable where TRepo : IMessagePoolRepository
                                                               where TSettings : IPoolDataUnitySettings, new()
    {
        #region props

        #region Context

        PoolDataContext context;

        #endregion
        #region Pool

        private IMessagePoolRepository _mp;
        public IMessagePoolRepository MP
        {
            get
            {
                return _mp = _mp ?? ( TRepo ) Activator.CreateInstance( typeof( TRepo ) , context );
            }
        }

        #endregion

        #endregion
        #region cst.

        public PoolDataUnity()
        {
            var settings = new TSettings();
            this.context = new PoolDataContext( settings.QueueTableName , settings.DBConnectionName );
        }

        #endregion

        #region publics

        public void Save()
        {
            context.SaveChanges();
        }

        #endregion
        #region IDisposable

        private bool disposed = false;
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
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion
    }
}
