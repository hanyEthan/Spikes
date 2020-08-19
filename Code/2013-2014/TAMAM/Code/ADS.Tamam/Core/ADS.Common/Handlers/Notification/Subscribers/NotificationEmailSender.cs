using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mail;

using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Utilities;
using ADS.Common.Handlers.Data;
using System.Threading.Tasks;

namespace ADS.Common.Handlers.Notification
{
    public class NotificationEmailSender : INotificationsListnerSubscriber
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "NotificationEmailSender"; } }

        private INotificationsEmailSenderDataHandler _DataHandler;
        private IPersonnelDetailsDataHandler _PersonnelDetailsDataHandler;

        private Task _MailQueueTask;
        private TimeSpan _MailQueueInterval = new TimeSpan( 0, 1, 0 );

        #endregion

        # region Constructor

        public NotificationEmailSender()
        {
            XLogger.Trace( "" );

            try
            {
                LoadDataHandlers();
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
                var To = _PersonnelDetailsDataHandler.GetEmailAddress( message.PersonId ).Result.ToString();
                if ( string.IsNullOrWhiteSpace( To ) )
                {
                    #region LOG
                    XLogger.Warning( string.Format( "NotificationEmailSender: Person with (Id: {0}) doesn't have an email address.", message.PersonId ) );
                    #endregion
                    return SubscriberStatus.Skipped;
                }

                string CCs = message.CCs;
                string Subject = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection, Constants.EmailNotificationMailSubject );
                string Body = GetMessageBody( message );
                var Attachments = Map( message.Attachments );

                if ( !TrySendMail( To, CCs, Subject, Body, Attachments ) )
                {
                    // insert into Mail Queue..
                    var emailMessage = new EmailMessage( message.Code, To, CCs, Subject, Body, message.AttachmentsSerialized );
                    var ok = _DataHandler.Save( emailMessage );
                    return ok ? SubscriberStatus.Succeed : SubscriberStatus.Failed;
                }

                return SubscriberStatus.Succeed;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return SubscriberStatus.Failed;
            }
        }
        public void PrepareSubscriber()
        {
            _MailQueueTask = new Task( ProcessMailQueue );
            _MailQueueTask.Start();
        }

        public string Token { get { return NotificationSubscribersTokens.EmailSubscriber; } }
        public SubscriberPriority Priority { get { return SubscriberPriority.Low; } }

        # endregion

        # region internals

        private bool TrySendMail( string To, string CCs, string Subject, string Body, List<XMail.XAttachment> Attachments )
        {
            try
            {
                var state = XMail.Send( To, CCs, Subject, Body, Attachments );
                return state;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        private async void ProcessMailQueue()
        {
            while ( true )
            {
                try
                {
                    XLogger.Info( "Mail Queue Task Started." );

                    CheckMailQueue();
                }
                catch ( Exception ex )
                {
                    XLogger.Error( string.Empty, ex );
                }

                XLogger.Info( "Mail Queue Task: Delay for: [{0}]", _MailQueueInterval.ToString() );
                await Task.Delay( _MailQueueInterval );
            }
        }
        private void CheckMailQueue()
        {
            XLogger.Trace( "" );

            try
            {
                var queuedMessages = _DataHandler.GetAll();
                foreach ( var message in queuedMessages )
                {
                    var Attachments = Map( message.Attachments );
                    var state = XMail.Send( message.To, message.CCs, message.Subject, message.Body, Attachments );
                    if ( state ) 
                    {
                        XLogger.Info( string.Format( "MailQueue: Mail with Id ({0}) sent successfully", message.Id ) );
                        _DataHandler.Delete( message.Id );
                    }
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : ", x );
            }
        }

        private void LoadDataHandlers()
        {
            var dataHandlerName = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection, Constants.EmailNotificationDataHandlerName );
            _PersonnelDetailsDataHandler = XReflector.GetInstance<IPersonnelDetailsDataHandler>( dataHandlerName );
            _DataHandler = new DataHandler();

            Initialized = _PersonnelDetailsDataHandler.Initialized && ( _DataHandler != null && _DataHandler.Initialized );
        }
        private string GetMessageBody( NotificationMessage message )
        {
            if ( string.IsNullOrWhiteSpace( message.MessageHTML ) )
            {
                var subject = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection, Constants.EmailNotificationMailSubject );
                var webURL = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection, Constants.EmailNotificationWebApplicationURL );
                var msgTemplate = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection, Constants.EmailNotificationMessageTemplate );

                var personName = _PersonnelDetailsDataHandler.GetName( message.PersonId );
                var personArabicName = _PersonnelDetailsDataHandler.GetArabicName( message.PersonId );

                string innerMessage = string.Format( "{0} <a href='{3}/{2}'>{1}</a> ", message.Message, message.ActionName, message.ActionUrl, webURL );
                string innerMessageArabic = string.Format( "{0} <a href='{3}/{2}'>{1}</a> ", message.MessageCultureVariant, message.ActionNameCultureVariant, message.ActionUrl, webURL );
                string body = string.Format( msgTemplate, personName.Result, innerMessage, subject, personArabicName.Result, innerMessageArabic, subject );

                return body;
            }
            else
            {
                return message.MessageHTML;
            }
        }
        private List<XMail.XAttachment> Map( List<NotificationAttachment> attachments )
        {
            if ( attachments == null || attachments.Count == 0 ) return null;
            return attachments.Select( x => new XMail.XAttachment( x.Name, x.MIMEType, x.Report ) ).ToList();
        }

        # endregion

        public static class XMail
        {
            # region props..

            public static string Host { get; private set; }
            public static int Port { get; private set; }
            public static string Username { get; private set; }
            public static string Password { get; private set; }

            public static bool EnableSSL { get; private set; }
            public static ConnectionSSLMode ConnectionMode { get; private set; }

            # endregion
            #region cst..

            static XMail()
            {
                try
                {
                    Host = Broker.ConfigurationHandler.GetValue(Constants.EmailNotificationSection, Constants.EmailNotificationHostName);
                    Port = ParseToInt(Broker.ConfigurationHandler.GetValue(Constants.EmailNotificationSection, Constants.EmailNotificationPort));
                    Username = Broker.ConfigurationHandler.GetValue(Constants.EmailNotificationSection, Constants.EmailNotificationUserName);
                    Password = Broker.ConfigurationHandler.GetValue(Constants.EmailNotificationSection, Constants.EmailNotificationPassword);
                    EnableSSL = ParseToBool(Broker.ConfigurationHandler.GetValue(Constants.EmailNotificationSection, Constants.EmailNotificationEnableSSL));

                    var isImplicitSSL = ParseToBool(Broker.ConfigurationHandler.GetValue(Constants.EmailNotificationSection, Constants.EmailNotificationImplicitSSL));
                    ConnectionMode = isImplicitSSL ? ConnectionSSLMode.Implicit : ConnectionSSLMode.Explicit;
                }
                catch (Exception x)
                {
                    //XLogger.Error("Exception : " + x);
                    //throw;
                }
            }

            #endregion

            public static bool Send( string To, string CCs, string Subject, string Body, List<XAttachment> Attachments )
            {
                try
                {
                    if ( ConnectionMode == ConnectionSSLMode.Implicit ) SendThroughImplicitConnection( To, CCs, Subject, Body, Attachments );
                    else if ( ConnectionMode == ConnectionSSLMode.Explicit ) SendThroughExplicitConnection( To, CCs, Subject, Body, Attachments );

                    return true;
                }
                catch ( Exception x )
                {
                    XLogger.Error( "Exception : " + x );
                    return false;
                }
            }
            public static void Configure(string host, int port, string username, string password, bool enableSSL, ConnectionSSLMode connectionMode)
            {
                Host = host;
                Port = port;
                Username = username;
                Password = password;
                EnableSSL = enableSSL;
                ConnectionMode = connectionMode;
            }

            # region internals

            #region Implicit ConnectionSSLMode

            private static void SendThroughImplicitConnection( string To, string CCs, string Subject, string Body, List<XAttachment> Attachments )
            {

                System.Web.Mail.MailMessage Message = new System.Web.Mail.MailMessage();
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpserver", Host );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpserverport", Port );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/sendusing", "2" );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1" );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/sendusername", Username );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/sendpassword", Password );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpusessl", EnableSSL );

                Message.From = Username;
                Message.To = To;
                Message.Subject = Subject;
                Message.BodyFormat = MailFormat.Html;
                Message.Body = Body;

                if ( !string.IsNullOrWhiteSpace( CCs ) ) Message.Cc = CCs;

                var tempAttachmentFiles = new List<string>();
                if ( Attachments.Any() )
                {
                    foreach ( var item in Attachments )
                    {
                        string filePath = GetFileName( item.Name );

                        File.WriteAllBytes( filePath, item.File );
                        tempAttachmentFiles.Add( filePath );

                        Message.Attachments.Add( new MailAttachment( filePath, MailEncoding.Base64 ) );
                    }
                }

                var smtpServer = string.Format( "{0}:{1}", Host, Port );
                SmtpMail.SmtpServer = smtpServer;
                SmtpMail.Send( Message );

                if ( tempAttachmentFiles.Any() ) DeleteTempFiles( tempAttachmentFiles );
            }
            private static string GetFileName( string fileName, int count = 0 )
            {
                if ( count > 0 ) fileName = fileName + "_" + count;

                var filePath = Path.Combine( Path.GetTempPath(), fileName );
                if ( File.Exists( filePath ) )
                {
                    GetFileName( fileName, count++ );
                }

                return filePath;
            }
            private static void DeleteTempFiles( List<string> paths )
            {
                if ( paths == null || paths.Count == 0 ) return;
                foreach ( var file in paths ) File.Delete( file );
            }

            #endregion
            #region Explicit ConnectionSSLMode

            private static void SendThroughExplicitConnection( string To, string CCs, string Subject, string Body, List<XAttachment> Attachments )
            {
                var smtpClient = new SmtpClient( Host, Port );
                smtpClient.Credentials = new NetworkCredential( Username, Password );
                smtpClient.EnableSsl = EnableSSL;

                var from = new MailAddress( Username );
                var to = new MailAddress( To );

                var mailMessage = new System.Net.Mail.MailMessage( from, to ) { IsBodyHtml = true };
                mailMessage.Subject = Subject;
                mailMessage.Body = Body;

                var ccs = string.IsNullOrWhiteSpace( CCs ) ? new string[] { } : CCs.Split( ',' );
                foreach ( string cc in ccs ) if ( !string.IsNullOrWhiteSpace( cc ) ) mailMessage.CC.Add( cc );

                if ( Attachments != null && Attachments.Count > 0 )
                {
                    foreach ( var item in Attachments ) mailMessage.Attachments.Add( new Attachment( new MemoryStream( item.File ), item.Name, item.MIMEType ) );
                }

                smtpClient.Send( mailMessage );
            }

            #endregion

            # endregion
            #region inner models

            public enum ConnectionSSLMode { Implicit, Explicit }
            public class XAttachment
            {
                public string Name { get; private set; }
                public string MIMEType { get; private set; }
                public byte[] File { get; private set; }

                public XAttachment( string name, string mimeType, byte[] file )
                {
                    this.Name = name;
                    this.MIMEType = mimeType;
                    this.File = file;
                }
            }

            #endregion
            # region helpers

            private static int ParseToInt( string S )
            {
                int Port = 25;
                if ( !int.TryParse( S, out Port ) ) Port = 25;
                return Port;
            }
            private static bool ParseToBool( string S )
            {
                bool Bool = false;
                if ( !bool.TryParse( S, out Bool ) ) Bool = false;
                return Bool;
            }

            # endregion
        }
    }
}