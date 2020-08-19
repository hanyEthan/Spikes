using System;
using XCore.Framework.Infrastructure.Messaging.Queues.Contracts;
using XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Context;
using XCore.Services.Geo.Core.DataLayer.Context;
using XCore.Services.Geo.Core.DataLayer.Contracts;
using XCore.Services.Geo.Core.DataLayer.Repositories;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Unity
{
    public class GeoDataUnity<TSettings> : IDisposable where TSettings : IGeoDataUnitySettings, new()
    {
        #region props

        #region Context

        GeoDataContext context;

        #endregion
        #region LocationEvent

        private ILocationEventRepository _LocationEvents;
        public ILocationEventRepository LocationEvents
        {
            get
            {
                return _LocationEvents = _LocationEvents ?? new LocationEventRepository(this.context);
            }
        }

        #endregion
        #region LocationEventLatest

        private ILocationEventLatestRepository _LocationEventsLatest;
        public ILocationEventLatestRepository LocationEventsLatest
        {
            get
            {
                return _LocationEventsLatest = _LocationEventsLatest ?? new LocationEventLatestRepository(this.context);
            }
        }

        #endregion

        #endregion
        #region cst.

        public GeoDataUnity()
        {
            var settings = new TSettings();
            this.context = new GeoDataContext( settings.DBConnectionName );
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
