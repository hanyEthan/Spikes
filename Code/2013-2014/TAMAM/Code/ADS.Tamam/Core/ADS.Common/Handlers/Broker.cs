using System;
using System.Collections.Generic;
using System.Dynamic;

using ADS.Common.Utilities;
using ADS.Common.Contracts;
using ADS.Common.Contracts.Security;
using ADS.Common.Contracts.AuditTrail;
using ADS.Common.Contracts.SystemCodes;
using ADS.Common.Contracts.Notification;
using ADS.Common.Contracts.Communication;
using ADS.Common.Contracts.Security.Authentication;
using ADS.Common.Handlers.Cache;
using ADS.Common.Handlers.Configuration;
using ADS.Common.Handlers.Settings;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Handlers;
using ADS.Common.Handlers.License.Contracts;
using ADS.Common.Handlers.License.Definition;

namespace ADS.Common.Handlers
{
    /// <summary>
    /// main facade layer
    /// </summary>
    public static class Broker
    {
        #region props. ...

        // Initialization status
        public static bool Initialized { get; private set; }
        public static bool Licensed { get; private set; }

        #endregion

        #region Services

        private static IConfigurationHandler ConfigurationBasicHandler { get; set; }
        public static IConfigurationHandler ConfigurationHandler { get; private set; }
        public static IAuthorizationHandler AuthorizationHandler { get; private set; }
        public static IAuditTrailHandler AuditTrailHandler { get; private set; }
        public static IMasterCodeHandler MasterCodeHandler { get; private set; }
        public static IDetailCodeHandler DetailCodeHandler { get; private set; }
        public static INotificationsSender NotificationsSender { get; private set; }
        public static INotificationsListner NotificationsListner { get; private set; }
        public static INotificationsHandler NotificationHandler { get; private set; }
        public static ICommunicationHandler CommunicationHandler { get; private set; }
        public static WorkflowEngine WorkflowEngine { get; set; }
        public static IAuthenticationService AuthenticationService { get; private set; }
        public static ILicenseHandler LicenseHandler { get; private set; }

        public static CacheHandler Cache { get; private set; }

        public static dynamic Settings { get; private set; }

        #endregion
        #region Events

        public static event EventHandler<ServiceInitializedEventArgs> InitializedConfigurationBasicHandler;
        public static event EventHandler<ServiceInitializedEventArgs> InitializedConfigurationHandler;
        public static event EventHandler<ServiceInitializedEventArgs> InitializedAuthorizationHandler;
        public static event EventHandler<ServiceInitializedEventArgs> InitializedAuditTrailHandler;
        public static event EventHandler<ServiceInitializedEventArgs> InitializedMasterCodeHandler;
        public static event EventHandler<ServiceInitializedEventArgs> InitializedNotificationHandler;
        public static event EventHandler<ServiceInitializedEventArgs> InitializedCommunicationHandler;
        public static event EventHandler<ServiceInitializedEventArgs> InitializedLicenseHandler;

        #endregion
        #region Models

        public class ServiceInitializedEventArgs : EventArgs
        {
            public object Name { get; internal set; }
        }

        #endregion
        #region privates

        private static SettingsHandler _SettingsHandler { get; set; }

        #endregion

        #region cst.

        /// <summary>
        /// initiating underlying components ...
        /// </summary>
        static Broker()
        {
            Initialize();
        }

        #endregion

        #region publics.

        public static bool Reinitialize()
        {
            Initialize();
            return Initialized;
        }

        #endregion
        #region Helpers

        private static void Initialize()
        {
            XLogger.Trace("ServiceBroker ...");

            try
            {
                Licensed = true;

                #region Settings

                _SettingsHandler = new SettingsHandler();
                Settings = new ExpandoObject();

                #endregion
                #region configuration

                // load basic config ...
                XLogger.Info("ServiceBroker ... loading basic configuration handler ...");
                ConfigurationBasicHandler = new ConfigurationBasicHandler();
                FireEvent(InitializedConfigurationBasicHandler, ConfigurationBasicHandler);

                // load config data ...
                XLogger.Info("ServiceBroker ... loading data handler for configuration handler ...");
                var ConfigurationDataHandler = InitializeHandler<IConfigurationDataHandler>(Constants.ConfigurationDatastoreTypeKey, "Broker", ConfigurationBasicHandler);
                if (!CheckHandlerStatus(ConfigurationDataHandler)) return;

                // load config ...
                XLogger.Info("ServiceBroker ... loading configuration handler ...");
                ConfigurationHandler = InitializeHandler<IConfigurationHandler>(Constants.ConfigurationTypeKey, "Broker", ConfigurationBasicHandler);
                if (ConfigurationHandler == null) return;

                // inject data layer into config layer ...
                XLogger.Info("ServiceBroker ... initializing configuration handler ...");
                ConfigurationHandler.DataHandler = ConfigurationDataHandler;

                // check config status
                if (!CheckHandlerStatus(ConfigurationHandler)) return;

                FireEvent(InitializedConfigurationHandler, ConfigurationHandler);

                #endregion

                // Note : at this level, we can depend on the main configuration layer to initialize the subsequent modules

                #region cache

                // load in memory module cache ...
                XLogger.Info("ServiceBroker ... initializing internal cache ...");
                Cache = new CacheHandler();
                XLogger.Info("ServiceBroker ... internal cache initialized.");

                #endregion
                #region security ...

                // load authorization data ...
                XLogger.Info("ServiceBroker ... loading authorization data handler ...");
                var authorizationDataHandler = InitializeHandler<IAuthorizationDataHandler>(Constants.AuthorizationDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(authorizationDataHandler)) return;

                // load authorization ...
                XLogger.Info("ServiceBroker ... loading authorization handler ...");
                AuthorizationHandler = InitializeHandler<IAuthorizationHandler>(Constants.AuthorizationTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (AuthorizationHandler == null) return;

                // inject data layer into authorization layer ...
                XLogger.Info("ServiceBroker ... initializing authorization handler ...");
                AuthorizationHandler.AuthorizationDataHandler = authorizationDataHandler;

                // check status ...
                if (!CheckHandlerStatus(AuthorizationHandler)) return;

                FireEvent(InitializedAuthorizationHandler, AuthorizationHandler);

                #endregion

                #region License

                // load license definitions
                var licenseDefinitions = InitializeHandler<IFeaturesDefinition>(Constants.LicenseDefinitionTypeKey, Constants.SectionBroker, ConfigurationHandler);

                //load License handler ...
                LicenseHandler = InitializeHandler<ILicenseHandler>(Constants.LicenseTypeKey, Constants.SectionBroker, ConfigurationHandler, licenseDefinitions.Definition);

                // check status ...
                if (!CheckHandlerStatus(LicenseHandler)) { Licensed = false; return; }

                // handle event in case of license expiration ...
                LicenseHandler.LicenseExpired += LicenseExpired;
                Licensed = LicenseHandler.IsValid;

                # endregion
                # region AuditTrail

                // load auditTrail data ...
                XLogger.Info("loading auditTrail data handler ...");
                var auditTrailDataHandler = InitializeHandler<IAuditTrailDataHandler>(Constants.AuditTrailDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(auditTrailDataHandler)) return;

                // load auditTrail ...
                AuditTrailHandler = InitializeHandler<IAuditTrailHandler>(Constants.AuditTrailTypeKey, Constants.SectionBroker, ConfigurationHandler, auditTrailDataHandler);

                // check status ...
                if (!CheckHandlerStatus(AuditTrailHandler)) return;

                FireEvent(InitializedAuditTrailHandler, AuditTrailHandler);

                # endregion
                # region System Codes

                // load MasterCode data ...
                XLogger.Info("ServiceBroker ... loading MasterCode data handler ...");
                var masterCodeDataHandler = InitializeHandler<IMasterCodeDataHandler>(Constants.MasterCodeDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(masterCodeDataHandler)) return;

                // load MasterCode ...
                MasterCodeHandler = InitializeHandler<IMasterCodeHandler>(Constants.MasterCodeTypeKey, Constants.SectionBroker, ConfigurationHandler, masterCodeDataHandler);
                if (MasterCodeHandler == null) return;

                if (!CheckHandlerStatus(MasterCodeHandler)) return;

                // load DetailCode data ...
                XLogger.Info("ServiceBroker ... loading DetailCode data handler ...");
                var detailCodeDataHandler = InitializeHandler<IDetailCodeDataHandler>(Constants.DetailCodeDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(detailCodeDataHandler)) return;

                // load MasterCode ...
                DetailCodeHandler = InitializeHandler<IDetailCodeHandler>(Constants.DetailCodeTypeKey, Constants.SectionBroker, ConfigurationHandler, detailCodeDataHandler);
                if (DetailCodeHandler == null) return;

                if (!CheckHandlerStatus(DetailCodeHandler)) return;

                FireEvent(InitializedMasterCodeHandler, MasterCodeHandler);

                # endregion
                # region Notifications

                // load Notification sender data handler ...
                XLogger.Info("loading Notification sender data handler ...");

                var notificationSenderDataHandler = InitializeHandler<INotificationsSenderDataHandler>(Constants.NotificationsSenderDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(notificationSenderDataHandler)) return;

                // load Notifications Sender ...
                NotificationsSender = InitializeHandler<INotificationsSender>(Constants.NotificationsSenderTypeKey, Constants.SectionBroker, ConfigurationHandler, notificationSenderDataHandler);
                if (NotificationsSender == null) return;

                // check status ...
                if (!CheckHandlerStatus(NotificationsSender)) return;


                //----------------- Notification Listener (Data + Handler)
                // load Notification (Listener) data ...
                XLogger.Info("loading Notification Listener data handler ...");
                var notificationListnerDataHandler = InitializeHandler<INotificationsListnerDataHandler>(Constants.NotificationsListnerDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(notificationListnerDataHandler)) return;

                // load Notification (Listener)
                NotificationsListner = InitializeHandler<INotificationsListner>(Constants.NotificationsListnerTypeKey, Constants.SectionBroker, ConfigurationHandler, notificationListnerDataHandler);
                if (NotificationsListner == null) return;
                // Set "TimerDueTime" from Configuration Handler
                //var dueTime = ConfigurationHandler.GetValue( Constants.SectionBroker , Constants.NotificationsListenerDueTime );
                //int dueTimeValue = TryToInt( dueTime ) == 0 ? 300000 : TryToInt( dueTime );
                //NotificationsListner.TimerDueTime = dueTimeValue;

                // check status ...
                if (!CheckHandlerStatus(NotificationsListner)) return;

                //----------------- Notifications List Sender (Data + Handler)

                // load INotificationsListSenderDataHandler (Dashboard Data Handler) data ...
                XLogger.Info("loading INotificationsListSenderDataHandler ...");
                var notificationsListSenderDataHandler = InitializeHandler<INotificationsListSenderDataHandler>(Constants.NotificationListSenderDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(notificationsListSenderDataHandler)) return;


                // ---------- Load Notification Subscribers
                XLogger.Info("loading Notification Subsribers ...");

                var subscribersToBeLoaded = Broker.ConfigurationHandler.GetValues(Constants.SectionBroker, Constants.NotificationListnerSubscribersPartialName);

                foreach (string subscriberTypeName in subscribersToBeLoaded)
                {
                    // comment , it need to send a data handler to subscriber, not all subscribers need data handler, so it can not initialize such as "Logger Subscriber"..
                    var subscriberHandler = Broker.InitializeHandler<INotificationsListnerSubscriber>(subscriberTypeName);
                    bool status = Broker.CheckHandlerStatus(subscriberHandler);
                    if (!status) return;

                    NotificationsListner.Subscribers.Add(subscriberHandler);
                }


                // load Notification Handler ...
                XLogger.Info("loading Notification data handler ...");
                var notificationDataHandler = InitializeHandler<INotificationsDataHandler>(Constants.NotificationsHandlerDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(notificationDataHandler)) return;

                List<INotificationTypeHandler> typeHandlers = new List<INotificationTypeHandler>();
                var notificationTypeHandlers = Broker.ConfigurationHandler.GetValues(Constants.SectionBroker, Constants.NotificationsTypeHandlerPartialName);
                foreach (string typeHandler in notificationTypeHandlers)
                {
                    var handler = Broker.InitializeHandler<INotificationTypeHandler>(typeHandler);
                    bool status = Broker.CheckHandlerStatus(handler);
                    if (!status) return;
                    typeHandlers.Add(handler);
                }

                NotificationHandler = InitializeHandler<INotificationsHandler>(Constants.NotificationsHandlerTypeKey, Constants.SectionBroker, ConfigurationHandler, notificationDataHandler, typeHandlers);
                if (NotificationHandler == null) return;

                // check status ...
                if (!CheckHandlerStatus(NotificationHandler)) return;

                FireEvent(InitializedNotificationHandler, NotificationHandler);

                # endregion
                # region Communications

                // load Communication handler ...
                XLogger.Info("loading Communication handler ...");

                CommunicationHandler = InitializeHandler<ICommunicationHandler>(Constants.CommunicationTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (CommunicationHandler == null) return;

                // check status ...
                if (!CheckHandlerStatus(CommunicationHandler)) return;

                FireEvent(InitializedCommunicationHandler, CommunicationHandler);

                # endregion
                # region Workflow

                // load workflow data ...
                XLogger.Info("loading workflow data handler ...");
                var workflowDataHandler = InitializeHandler<IWorkflowDataHandler>(Constants.WorkflowDatastoreTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(workflowDataHandler)) return;

                // load workflow engine ...
                WorkflowEngine = new WorkflowEngine(workflowDataHandler);

                // check status ...
                if (!WorkflowEngine.Initialized) return;

                # endregion
                # region Authentication

                XLogger.Info("loading authentication handlers ...");

                // identity provider ...
                var identityProvider = InitializeHandler<IIdentityProvider>(Constants.AuthenticationIdentityProviderTypeKey, Constants.SectionBroker, ConfigurationHandler);
                if (!CheckHandlerStatus(identityProvider)) return;

                // authentication providers ...
                var authenticationProviders = new Dictionary<string, IAuthenticationProvider>();
                var authenticationProvidersToBeLoaded = Broker.ConfigurationHandler.GetValues(Constants.SectionBroker, Constants.AuthenticationProvidersPartialName);
                foreach (string provider in authenticationProvidersToBeLoaded)
                {
                    var authenticationProvider = Broker.InitializeHandler<IAuthenticationProvider>(provider);
                    bool status = Broker.CheckHandlerStatus(authenticationProvider);
                    if (!status) return;

                    authenticationProviders.Add(authenticationProvider.Mode, authenticationProvider);
                }

                // authentication service ...
                var authenticationService = InitializeHandler<IAuthenticationService>(Constants.AuthenticationServiceTypeKey, Constants.SectionBroker, ConfigurationHandler);
                authenticationService.IdentityProvider = identityProvider;
                authenticationService.AuthenticationProviders = authenticationProviders;

                if (!CheckHandlerStatus(authenticationService)) return;
                AuthenticationService = authenticationService;

                # endregion

                XLogger.Info("ServiceBroker started successfully.");
                Initialized = Licensed = true;
            }
            catch (Exception x)
            {
                XLogger.Error("ServiceBroker FAILED to start.");
                XLogger.Error("ServiceBroker ... Exception: " + x);

                //ExceptionHandler.Handle( x );
            }
        }
        public static T InitializeHandler<T>(string handlerKey, string appId, IConfigurationHandler handlerKeySource, params object[] args) where T : class, IBaseHandler
        {
            return XReflector.GetInstance<T>(handlerKeySource.GetValue(appId, handlerKey), args);
        }
        public static T InitializeHandler<T>(string handlerTypeName, params object[] args) where T : class, IBaseHandler
        {
            return XReflector.GetInstance<T>(handlerTypeName, args);
        }
        public static bool CheckHandlerStatus(IBaseHandler handler)
        {
            if (handler != null && handler.Initialized)
            {
                XLogger.Info("ServiceBroker ... the handler with the key [{0}] started successfully.", handler.Name);
                return true;
            }
            else
            {
                if (handler == null) XLogger.Error("ServiceBroker ... the currently loaded handler is NOT initialized.");
                else XLogger.Error("ServiceBroker ... the handler with the key [{0}] is NOT initialized.", handler.Name);

                return false;
            }
        }
        public static int TryToInt(string str)
        {
            int integerValue;
            var isInteger = int.TryParse(str, out integerValue);
            return isInteger ? integerValue : 0;
        }
        private static void FireEvent( EventHandler<ServiceInitializedEventArgs> Event , IBaseHandler handler )
        {
            if ( handler != null && Event != null ) Event( handler , new ServiceInitializedEventArgs() { Name = handler.Name } );
        }

        private static void LicenseExpired(object sender, EventArgs e)
        {
            Initialized = false;
            Licensed = false;

            //////ConfigurationBasicHandler = null;
            //////ConfigurationHandler = null;
            //////AuthorizationHandler = null;
            //////AuditTrailHandler = null;
            //////MasterCodeHandler = null;
            //////DetailCodeHandler = null;
            //////NotificationsSender = null;
            //////NotificationsListner = null;
            //////NotificationHandler = null;
            //////CommunicationHandler = null;
            //////WorkflowEngine = null;
            //////AuthenticationService = null;
            //LicenseHandler = null;
        }

        #endregion
    }
}
