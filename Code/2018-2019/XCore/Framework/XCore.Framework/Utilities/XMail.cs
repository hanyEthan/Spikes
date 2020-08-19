using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Framework.Utilities
{
    public static class XMail
    {
        //public static bool SendEmail( string from , string to , string subject , string body , string smtpServer , int port , bool isSSL , string username , string password )
        //{
        //    if ( string.IsNullOrEmpty( smtpServer ) || string.IsNullOrEmpty( from ) || string.IsNullOrEmpty( to ) ) return false;

        //    try
        //    {
        //        var message = new MailMessage( from , to , subject , body );

        //        var smtp = new SmtpClient( smtpServer , port );
        //        smtp.EnableSsl = isSSL;
        //        smtp.Credentials = new NetworkCredential( username , password );
        //        smtp.Send( message );

        //        return true;
        //    }
        //    catch ( Exception x )
        //    {
        //        XLogger.Error( "Exception : " + x );
        //        return false;
        //    }
        //}
        public static bool SendEmail( string from , string[] to , string[] ccs , string subject , string body , bool isBodyHTML , string smtpServer , int port , bool isSSL , string username , string password , List<XMailAttachment> attachments )
        {
            if ( string.IsNullOrEmpty( smtpServer ) || string.IsNullOrEmpty( from ) || to == null || to.Length == 0 ) return false;

            try
            {
                var smtp = new SmtpClient( smtpServer , port );
                smtp.EnableSsl = isSSL;
                smtp.Credentials = new NetworkCredential( username , password );

                var message = new MailMessage();
                message.From = new MailAddress( from );
                foreach ( var target in to ) { message.To.Add( new MailAddress( target ) ); }
                foreach ( var target in ccs ) { message.CC.Add( new MailAddress( target ) ); }
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isBodyHTML;

                if ( attachments != null && attachments.Any() )
                {
                    foreach ( var item in attachments )
                    {
                        Attachment attachment = new Attachment( item.Stream , item.FileName );
                        message.Attachments.Add( attachment );
                    }
                }

                smtp.Send( message );
                return true;
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return false;
            }
        }
    }

    public class XMailAttachment
    {

        public XMailAttachment( string fileName , Stream stream )
        {
            Stream = stream;
            FileName = fileName;
        }

        public string FileName { get; private set; }
        public Stream Stream { get; private set; }
    }
}
