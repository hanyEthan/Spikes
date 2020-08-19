using System;
using System.Linq;
using System.Collections.Generic;

using Telerik.OpenAccess;

using ADS.Common.Contracts;
using ADS.Common.Handlers;
using ADS.Common.Handlers.Data.ORM;
using ADS.Common.Models.Enums;
using ADS.Common.Utilities;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Models;

namespace ADS.Common.Bases.MessageQueuing.Handlers.DB.DataLayer
{
    public class MQDBDataLayer<T> : IDataAccessHandler where T : IMQMessageContent
    {
        #region props ...

        public bool Initialized { get; private set; }
        public string Name { get { return "MQDBDataLayer"; } }

        #endregion
        #region cst.

        public MQDBDataLayer()
        {
            try
            {
                using ( var context = GetDataContext() )
                {
                    var conn = context.Connection;
                    Initialized = true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                Initialized = false;
            }
        }
        
        #endregion
        #region Helpers

        private bool IsOracle()
        {
            return Broker.Settings.Datastore.Type == DatastoreType.Oracle;
        }
        private bool IsOracle( DomainDataContext dbContext )
        {
            return dbContext.Metadata.BackendType != Telerik.OpenAccess.Metadata.Backend.MsSql;
        }
        private DomainDataContext GetDataContext()
        {
            var dbContext = new DomainDataContext();

            dbContext.BackendInfo.MaximumNumberOfInValues = Math.Max( Broker.Settings.Datastore.MaximumNumberOfInValues , dbContext.BackendInfo.MaximumNumberOfInValues );
            dbContext.BackendInfo.MaximumNumberOfQueryParameters = Math.Max( Broker.Settings.Datastore.MaximumNumberOfQueryParameters , dbContext.BackendInfo.MaximumNumberOfQueryParameters );

            return dbContext;
        }

        #endregion

        #region Publics

        public MQMessage Get( Guid id )
        {
            try
            {
                XLogger.Trace( "" );
                using ( var context = GetDataContext() )
                {
                    var message = context.MQMessages.Where( x => x.Type == typeof( T ).ToString() )
                                                    .OrderBy( x => x.CreationTime ).FirstOrDefault();

                    return context.CreateDetachedCopy( message );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return null;
            }
        }
        public MQMessage GetNext()
        {
            try
            {
                XLogger.Trace( "" );
                using ( var context = GetDataContext() )
                {
                    var messages = context.MQMessages.Where( x => x.Type == typeof( T ).ToString() && x.Status == MQMessageStatus.UnProcessed )
                                                     .OrderBy( x => x.CreationTime ).FirstOrDefault();

                    return messages != null ? context.CreateDetachedCopy( messages, msg => msg.ContentSerialized ) : null;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return null;
            }
        }
        public List<MQMessage> GetAll()
        {
            try
            {
                XLogger.Trace( "" );
                using ( var context = GetDataContext() )
                {
                    var messages = context.MQMessages.Where( x => x.Type == typeof( T ).ToString() && x.Status == MQMessageStatus.UnProcessed )
                                                     .OrderBy( x => x.CreationTime ).ToList();

                    return context.CreateDetachedCopy( messages );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return null;
            }
        }
        public int GetCount()
        {
            try
            {
                XLogger.Trace( "" );
                using ( var context = GetDataContext() )
                {
                    return context.MQMessages.Where( x => x.Type == typeof( T ).ToString() && x.Status == MQMessageStatus.UnProcessed ).Count();
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return -1;
            }
        }

        public bool Save( MQMessage message )
        {
            try
            {
                XLogger.Trace( "" );
                using ( var context = GetDataContext() )
                {
                    context.Add( message );
                    context.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: " , x );
                return false;
            }
        }
        public bool Edit( MQMessage message )
        {
            try
            {
                XLogger.Trace( "" );
                using ( var context = GetDataContext() )
                {
                    context.AttachCopy( message );
                    context.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: " , x );
                return false;
            }
        }
        public bool Edit( List<MQMessage> messages )
        {
            try
            {
                XLogger.Trace( "" );
                using ( var context = GetDataContext() )
                {
                    context.AttachCopy( messages );
                    context.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception: " , x );
                return false;
            }
        }
        public bool Delete( MQMessage message )
        {
            XLogger.Info( "" );

            try
            {
                using ( var context = GetDataContext() )
                {
                    context.Delete( message );
                    context.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return false;
            }
        }
        public bool Delete( List<MQMessage> messages )
        {
            XLogger.Info( "" );

            try
            {
                using ( var context = GetDataContext() )
                {
                    context.Delete( messages );
                    context.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return false;
            }
        }
        public bool Delete( Guid id )
        {
            try
            {
                XLogger.Info( "" );
                using ( var context = GetDataContext() )
                {
                    var deleted = context.MQMessages.Where( x => x.Id == id ).DeleteAll();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return false;
            }
        }
        public bool Delete( List<Guid> ids )
        {
            try
            {
                XLogger.Info( "" );
                using ( var context = GetDataContext() )
                {
                    var deleted = context.MQMessages.Where( x => ids.Contains( x.Id ) ).DeleteAll();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " , x );
                return false;
            }
        }

        #endregion
    }
}
