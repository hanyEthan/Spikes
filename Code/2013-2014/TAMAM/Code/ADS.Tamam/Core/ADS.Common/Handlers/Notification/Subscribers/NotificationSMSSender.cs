using System;

using ADS.Common.Handlers.Data;
using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Notification.Subscribers
{
    public class NotificationSMSSender : INotificationsListnerSubscriber
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "NotificationSMSSender"; } }

        private IPersonnelDetailsDataHandler _DataHandlerForPersonnel;
        private ISMSNotificationsDataHandler _DataHandlerForMessages;

        private string _WebURL = "";
        private string _MsgTemplate = "";
        private string _Subject = "";

        #endregion
        # region Constructor

        public NotificationSMSSender()
        {
            XLogger.Trace( "" );

            try
            {
                XLogger.Info( "Loading config ..." );

                _WebURL = Broker.ConfigurationHandler.GetValue( Constants.SMSNotificationSection , Constants.SMSNotificationWebApplicationURL );
                _MsgTemplate = Broker.ConfigurationHandler.GetValue( Constants.SMSNotificationSection , Constants.SMSNotificationMessageTemplate );
                _Subject = Broker.ConfigurationHandler.GetValue( Constants.SMSNotificationSection , Constants.SMSNotificationSubject );

                XLogger.Info( "Loading DataHandler..." );

                var dataHandlerName = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationDataHandlerName );
                _DataHandlerForPersonnel = XReflector.GetInstance<IPersonnelDetailsDataHandler>( dataHandlerName );

                _DataHandlerForMessages = new DataHandler() as ISMSNotificationsDataHandler;
                Initialized = _DataHandlerForPersonnel.Initialized && _DataHandlerForPersonnel.Initialized;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                Initialized = false;
            }
        }

        # endregion
        # region Publics
        
        public SubscriberStatus Send( NotificationMessage message )
        {
            XLogger.Trace( "" );

            try
            {
                var cellNumber = _DataHandlerForPersonnel.GetCellNumber( message.PersonId ).Result.ToString();
                var personName = _DataHandlerForPersonnel.GetName( message.PersonId ).Result;
                var personArabicName = _DataHandlerForPersonnel.GetArabicName( message.PersonId );

                if ( string.IsNullOrWhiteSpace( cellNumber ) )
                {
                    XLogger.Warning( string.Format( "NotificationSMSSender: Person with (Id: {0}), (Name: {1}) may haven't a Phone Number." , message.PersonId , personName ) );
                    return SubscriberStatus.Skipped;
                }

                string messageEnglish = string.Format( "{0}, {1}: {2}/{3}" , message.Message , message.ActionName , _WebURL , message.ActionUrl );
                string messageArabic = string.Format( "{0}, {1}: {2}/{3}" , message.MessageCultureVariant , message.ActionNameCultureVariant , _WebURL , message.ActionUrl );
                string messageCMBN = string.Format( _MsgTemplate , personName.ToString() , messageEnglish , _Subject , personArabicName.Result.ToString() , messageArabic , _Subject );

                var sms = new SMSMessage( cellNumber , messageCMBN , "en-US" , DateTime.Now );
                var saved = _DataHandlerForMessages.CreateMessage( sms );

                return saved ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return SubscriberStatus.Failed;
            }
        }
        public void PrepareSubscriber() { }

        public string Token { get { return NotificationSubscribersTokens.SMSSubscriber; } }
        public SubscriberPriority Priority { get { return SubscriberPriority.Low; } }

        # endregion
    }
}