using System;
using System.Collections.Generic;
using System.Linq;
using XCore.Utilities.Infrastructure.Messaging.Pools.Contracts;
using XCore.Utilities.Infrastructure.Messaging.Pools.Models;
using XCore.Utilities.Infrastructure.Messaging.Pools.Repositories;
using XCore.Utilities.Infrastructure.Messaging.Pools.Repositories.Unity;
using XCore.Utilities.Utilities;

namespace XCore.Utilities.Infrastructure.Messaging.Pools.Handlers
{
    #region MessagePoolHandler

    public class MessagePoolHandler : MessagePoolHandler<MessagePoolRepository, PoolDataUnitySettings>
    {
    }

    #endregion
    #region MessagePoolHandler<TRepo, TSettings>

    public class MessagePoolHandler<TRepo, TSettings> : IMessagePoolSender, IMessagePoolReader where TRepo : IMessagePoolRepository
                                                                                               where TSettings : IPoolDataUnitySettings, new()
    {
        #region IMessagePoolSender

        public int Create( PoolMessage message )
        {
            try
            {
                using ( var dataHandler = new PoolDataUnity<TRepo , TSettings>() )
                {
                    dataHandler.MP.Create( message );
                    dataHandler.Save();
                }
                return message.Id;
            }
            catch
            {
                throw;
            }
        }

        #endregion
        #region IMessagePoolReader

        public List<PoolMessage> Get( PoolMessageSearchCriteria filters )
        {
            try
            {
                using ( var dataHandler = new PoolDataUnity<TRepo , TSettings>() )
                {
                    // query ...
                    var messages = dataHandler.MP.Get( filters );

                    // pop ...
                    if ( filters.PopMessages && messages != null && messages.Count != 0 )
                    {
                        dataHandler.MP.Hold( messages );
                        dataHandler.Save();
                    }

                    // ...
                    return messages;
                }
            }
            catch
            {
                throw;
            }
        }
        public bool Restore( List<string> ids )
        {
            try
            {
                using ( var dataHandler = new PoolDataUnity<TRepo , TSettings>() )
                {
                    dataHandler.MP.Restore( ids );
                    dataHandler.Save();

                    return true;
                }
            }
            catch ( Exception )
            {
                throw;
            }
        }
        public bool Delete( List<string> ids )
        {
            try
            {
                using ( var dataHandler = new PoolDataUnity<TRepo , TSettings>() )
                {
                    dataHandler.MP.Delete( ids );
                    dataHandler.Save();

                    return true;
                }
            }
            catch ( Exception )
            {
                throw;
            }
        }

        #endregion
    }

    #endregion
}
