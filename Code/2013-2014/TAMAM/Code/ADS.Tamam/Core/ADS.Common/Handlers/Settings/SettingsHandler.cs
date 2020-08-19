using System;
using System.Dynamic;
using ADS.Common.Contracts;
using ADS.Common.Models.Enums;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Settings
{
    public class SettingsHandler
    {
        #region cst ...

        public SettingsHandler()
        {
            Broker.InitializedConfigurationBasicHandler += Broker_InitializedConfigurationBasicHandler;
            //Broker.InitializedConfigurationHandler += Broker_InitializedConfigurationHandler;

            //Broker.InitializedAuditTrailHandler += Broker_InitializedAuditTrailHandler;
            //Broker.InitializedAuthorizationHandler += Broker_InitializedAuthorizationHandler;
            //Broker.InitializedCommunicationHandler += Broker_InitializedCommunicationHandler;
            //Broker.InitializedMasterCodeHandler += Broker_InitializedMasterCodeHandler;
            //Broker.InitializedNotificationHandler += Broker_InitializedNotificationHandler;
        }

        #endregion
        #region internals

        void Broker_InitializedConfigurationBasicHandler( object sender , Broker.ServiceInitializedEventArgs e )
        {
            try
            {
                Broker.Settings.RoundPolicy = new ExpandoObject();
                Broker.Settings.RoundPolicy.RoundTo = 2;

                Broker.Settings.Datastore = new ExpandoObject();
                Broker.Settings.Datastore.Type = DatastoreType.Unknown;
                Broker.Settings.Datastore.MaximumNumberOfInValues = 1000;
                Broker.Settings.Datastore.MaximumNumberOfQueryParameters = 1000;
               
                var service = sender as IConfigurationHandler;
                if ( service == null ) return;

                var backend = service.GetValue( "" , "BackEnd" ) ?? "";
                switch ( backend.ToLower() )
                {
                    case "mssql":
                        {
                            Broker.Settings.Datastore.Type = DatastoreType.SQLServer;
                            break;
                        }
                    case "oracle":
                        {
                            Broker.Settings.Datastore.Type = DatastoreType.Oracle;
                            break;
                        }
                    case "":
                    default:
                        {
                            Broker.Settings.Datastore.Type = DatastoreType.Unknown;
                            break;
                        }
                }
            }
            catch( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }

        private void Broker_InitializedAuditTrailHandler( object sender , Broker.ServiceInitializedEventArgs e )
        {
            throw new System.NotImplementedException();
        }
        void Broker_InitializedNotificationHandler( object sender , Broker.ServiceInitializedEventArgs e )
        {
            throw new System.NotImplementedException();
        }
        void Broker_InitializedMasterCodeHandler( object sender , Broker.ServiceInitializedEventArgs e )
        {
            throw new System.NotImplementedException();
        }
        void Broker_InitializedConfigurationHandler( object sender , Broker.ServiceInitializedEventArgs e )
        {
            throw new System.NotImplementedException();
        }
        void Broker_InitializedCommunicationHandler( object sender , Broker.ServiceInitializedEventArgs e )
        {
            throw new System.NotImplementedException();
        }
        void Broker_InitializedAuthorizationHandler( object sender , Broker.ServiceInitializedEventArgs e )
        {
            throw new System.NotImplementedException();
        }
   
        #endregion
    }
}
