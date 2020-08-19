using System;
using System.Collections.Generic;

using ADS.Common.Handlers;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data;

namespace ADS.Tamam.Common.Handlers
{
    public static class TamamDataBroker
    {
        #region Properties ...

        // Initialization status
        public static bool Initialized { get; private set; }

        #endregion
        #region Services

        private static Dictionary<string , IDataAccessHandler> _DataAccessHandlers = new Dictionary<string , IDataAccessHandler>();
        public static Dictionary<string , IDataAccessHandler> DataAccessHandlers { get { return _DataAccessHandlers; } }

        #endregion

        #region cst.

        /// <summary>
        /// initiating underlying components ...
        /// </summary>
        static TamamDataBroker()
        {
            XLogger.Info( "TamamBroker ..." );

            try
            {
                if ( !Broker.Initialized ) return;
                Broker.LicenseHandler.LicenseExpired += LicenseExpired;

                #region data access handlers ...

                var DataHandlersToBeLoaded = Broker.ConfigurationHandler.GetValues( "Broker" , TamamConstants.DataHandlersPartialName );
                foreach ( string dataHandlerTypeName in DataHandlersToBeLoaded )
                {
                    var dataHandler = Broker.InitializeHandler<IDataAccessHandler>( dataHandlerTypeName );
                    bool status = Broker.CheckHandlerStatus( dataHandler );
                    if ( !status ) return;

                    DataAccessHandlers.Add( dataHandler.Name , dataHandler );
                }

                #endregion

                XLogger.Info( "TamamBroker started successfully." );
                Initialized = true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "TamamBroker FAILED to start." );
                XLogger.Error( "TamamBroker ... Exception: " + x );

                //ExceptionHandler.Handle( x );
            }
        }

        #endregion
        #region Helpers

        public static T GetRegisteredDataLayer<T>( string key ) where T : class
        {
            IDataAccessHandler dataHandler;
            DataAccessHandlers.TryGetValue( key , out dataHandler );
            return dataHandler as T;
        }
        private static void LicenseExpired(object sender, EventArgs e)
        {
            Initialized = false;

            //_DataAccessHandlers = null;
        }

        #endregion
    }
}
