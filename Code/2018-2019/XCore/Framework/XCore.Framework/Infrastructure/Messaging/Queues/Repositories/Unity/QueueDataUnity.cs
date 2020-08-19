using System;
using XCore.Framework.Infrastructure.Messaging.Queues.Contracts;
using XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Context;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Unity
{
    public class QueueDataUnity<TRepo, TSettings> : IDisposable where TRepo : IQueueRepository 
                                                                where TSettings : IQueueDataUnitySettings , new()
    {
        #region props

        #region Context

        QueueDataContext context;

        #endregion
        #region MQ

        private IQueueRepository _mq;
        public IQueueRepository MQ
        {
            get
            {
                return _mq = _mq ?? ( TRepo ) Activator.CreateInstance( typeof( TRepo ) , context );
            }
        }

        #endregion

        #endregion
        #region cst.

        public QueueDataUnity()
        {
            var settings = new TSettings();
            this.context = new QueueDataContext( settings.QueueTableName , settings.DBConnectionName );
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
