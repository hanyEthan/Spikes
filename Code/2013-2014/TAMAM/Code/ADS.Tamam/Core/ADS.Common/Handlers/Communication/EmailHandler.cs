using ADS.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Mail;

namespace ADS.Common.Handlers.Communication
{
    public class EmailHandler
    {
        #region Properties

        public bool Initialized { get; private set; }

        // fields..
        private bool _ImplicitSSL;
        private bool _EnableSSL;
        private string _Host = "";
        private int _Port = 25;
        private string _Username = "";
        private string _Password = "";
        private string _Subject = "";
        private string _WebURL = "";
        private string _MsgTemplate = "";

        #endregion
        # region Constructor

        public EmailHandler()
        {
            XLogger.Trace( "" );

            try
            {
                XLogger.Info( "Loading SmtpClient..." );

                // get email settings..
                _ImplicitSSL = Parse( Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationImplicitSSL ) );
                _EnableSSL = Parse( Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationEnableSSL ) );
                _Port = ParsePort( Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationPort ) );
                _Host = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationHostName );
                _Username = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationUserName );
                _Password = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationPassword );
                _Subject = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationMailSubject );
                _WebURL = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationWebApplicationURL );
                _MsgTemplate = Broker.ConfigurationHandler.GetValue( Constants.EmailNotificationSection , Constants.EmailNotificationMessageTemplate );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                Initialized = false;
            }
        }

        # endregion
        # region Publics

        public bool Send(string[] emails, string[] ccs, string body, List<XMailAttachment> attachments)
        {
            XLogger.Trace( "" );

            try
            {
                string mailCCs = "";
                foreach ( var target in ccs ) { mailCCs += target + ","; }
                mailCCs = mailCCs.TrimEnd( ',' );

                foreach ( var emailTarget in emails )
                {
                    if ( _ImplicitSSL )
                    {
                        if ( !ImplicitSSL_Send( emailTarget , body , mailCCs , attachments ) ) return false;
                    }
                    else
                    {
                        if ( !ExplicitSSL_Send( emailTarget , body , mailCCs , attachments ) ) return false;
                    }
                    
                    mailCCs = "";
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        # endregion

        # region helpers

        private bool Parse( string S )
        {
            bool Bool = false;
            if ( !bool.TryParse( S , out Bool ) ) Bool = false;
            return Bool;
        }
        private int ParsePort( string S )
        {
            int Port = 25;
            if ( !int.TryParse( S , out Port ) ) Port = 25;
            return Port;
        }

        # endregion
        # region Send : Implicit SSL

        public bool ImplicitSSL_Send( string To , string Body , string CCs , object Attachments )
        {
            try
            {
                System.Web.Mail.MailMessage Message = new System.Web.Mail.MailMessage();
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpserver" , _Host );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpserverport" , _Port );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/sendusing" , "2" );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate" , "1" );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/sendusername" , _Username );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/sendpassword" , _Password );
                Message.Fields.Add( "http://schemas.microsoft.com/cdo/configuration/smtpusessl" , _EnableSSL );

                Message.From = _Username;
                Message.To = To;
                Message.Subject = _Subject;
                Message.BodyFormat = MailFormat.Html;
                Message.Body = Body;
                Message.BodyEncoding = UnicodeEncoding.UTF8;

                if ( !string.IsNullOrWhiteSpace( CCs ) ) Message.Cc = CCs;

                var filesToBeDeleted = new List<string>();

                var notificationAttachments = Attachments as List<XMailAttachment>;
                if ( notificationAttachments != null && notificationAttachments.Any() )
                {
                    foreach ( var item in notificationAttachments )
                    {
                        string filePath = GetFileName( item.FileName );

                        File.WriteAllBytes( filePath , ( item.Stream as MemoryStream ).ToArray() );
                        filesToBeDeleted.Add( filePath );

                        Message.Attachments.Add( new MailAttachment( filePath , MailEncoding.Base64 ) );
                    }
                }

                var smtpServer = _Host + ":" + _Port;
                SmtpMail.SmtpServer = smtpServer;
                SmtpMail.Send( Message );

                // delete all created files ...
                DeleteTempFiles( filesToBeDeleted );

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }

        }
        private string GetFileName( string fileName , int count = 0 )
        {
            if ( count > 0 ) fileName = fileName + "_" + count;

            var filePath = Path.Combine( Path.GetTempPath() , fileName );
            if ( File.Exists( filePath ) )
            {
                GetFileName( fileName , count+1 );
            }

            return filePath;
        }
        private void DeleteTempFiles( List<string> files )
        {
            if ( files == null || files.Count == 0 ) return;
            foreach ( var file in files ) File.Delete( file );
        }

        # endregion
        # region Send : Explicit SSL

        public bool ExplicitSSL_Send( string To , string Body , string CCs , object Attachments )
        {
            try
            {
                var smtpClient = new SmtpClient( _Host , _Port );
                smtpClient.Credentials = new NetworkCredential( _Username , _Password );
                smtpClient.EnableSsl = _EnableSSL;

                var from = new MailAddress( _Username );
                var to = new MailAddress( To );

                var mailMessage = new System.Net.Mail.MailMessage( from , to ) { IsBodyHtml = true };
                mailMessage.Subject = _Subject;
                mailMessage.Body = Body;

                if ( !string.IsNullOrWhiteSpace( CCs ) )
                {
                    var CCEmails = CCs.Split( ',' );
                    foreach ( string mail in CCEmails )
                    {
                        if ( !string.IsNullOrWhiteSpace( mail ) ) mailMessage.CC.Add( mail );
                    }
                }

                var notificationAttachments = Attachments as List<XMailAttachment>;
                if ( notificationAttachments != null && notificationAttachments.Any() )
                {
                    foreach ( var item in notificationAttachments )
                    {
                        var attachment = new Attachment( item.Stream , item.FileName );
                        mailMessage.Attachments.Add( attachment );
                    }
                }

                smtpClient.Send( mailMessage );
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        # endregion
    }
}
