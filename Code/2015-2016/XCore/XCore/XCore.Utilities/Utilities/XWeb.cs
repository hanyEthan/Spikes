using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.ServiceModel.Web;
using System.IO.Compression;

namespace XCore.Utilities.Utilities
{
    public static class XWeb
    {
        #region nested. 

        public static class Session
        {
            public static object Get( string name )
            {
                if ( !IsValid( name ) ) return null;
                return HttpContext.Current.Session[name];
            }
            public static void Set( string name , object value )
            {
                if ( !IsValid( name ) ) return;
                HttpContext.Current.Session[name] = value;
            }
            public static void Add( string name , object value )
            {
                if ( !IsValid( name ) ) return;
                HttpContext.Current.Session.Add( name , value );
            }
            public static void Clear()
            {
                if ( !IsValid() ) return;

                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();
            }
            public static void Clear( string name )
            {
                if ( !IsValid( name ) ) return;
                HttpContext.Current.Session.Remove( name );
            }

            private static bool IsValid()
            {
                return HttpContext.Current != null;
            }
            private static bool IsValid( string name )
            {
                return HttpContext.Current != null && !string.IsNullOrWhiteSpace( name );
            }
        }

        #endregion
        #region Fields

        private const int StreamBuffer = 1024;

        private static readonly Regex RegexBetweenTags = new Regex( @">\s+" , RegexOptions.Compiled );
        private static readonly Regex RegexLineBreaks = new Regex( @"\n\s+" , RegexOptions.Compiled );

        #endregion

        public static string CompressHttpContent( string content , FileContentType contentType )
        {
            switch ( contentType )
            {
                case FileContentType.HTML: return RemoveWhitespaceFromHtml( content );
                case FileContentType.CSS: return RemoveWhitespaceFromCss( content );
                case FileContentType.JavaScript: return RemoveWhitespaceFromJavaScript( content );
                default: return null;
            }
        }
        public static void CompressHttpResponse( HttpContext httpContext )
        {
            if ( httpContext != null )
            {
                if ( IsHttpRequestEncodingAccepted( HttpEncodingType.Deflate ) )
                {
                    httpContext.Response.Filter = new DeflateStream( httpContext.Response.Filter , CompressionMode.Compress );
                    SetHttpResponseEncoding( HttpEncodingType.Deflate );
                }
                else if ( IsHttpRequestEncodingAccepted( HttpEncodingType.GZIP ) )
                {
                    httpContext.Response.Filter = new GZipStream( httpContext.Response.Filter , CompressionMode.Compress );
                    SetHttpResponseEncoding( HttpEncodingType.GZIP );
                }
            }
        }
        public static bool ExecuteHttp( string command , string url , string contentType , NameValueCollection headers , object body , out Stream result )
        {
            result = null;

            try
            {
                result = ExecuteHttp( command , url , contentType , headers , body );
                return result != null;
            }
            catch ( Exception x )
            {
                NLogger.Error( "XUtilities.XWeb.ExecuteHttp ... Exception: " + x );
                return false;
            }
        }
        public static bool ExecuteHttp( string command , string url , string contentType , NameValueCollection headers , object body , out string result )
        {
            result = null;

            try
            {
                Stream stream = ExecuteHttp( command , url , contentType , headers , body );
                if ( stream == null ) return false;

                StreamReader reader = new StreamReader( stream );
                result = reader.ReadToEnd();

                return true;
            }
            catch ( Exception x )
            {
                return NLogger.Error( "XUtilities.XWeb.ExecuteHttp ... Exception: " + x );
            }
        }
        public static bool ExecuteHttp( string command , string url , string contentType , NameValueCollection headers , object body , out byte[] result )
        {
            result = null;

            try
            {
                Stream stream = ExecuteHttp( command , url , contentType , headers , body );
                if ( stream == null ) return false;

                result = new byte[( int ) stream.Length];
                stream.Read( result , 0 , ( int ) stream.Length );

                return true;
            }
            catch ( Exception x )
            {
                return NLogger.Error( "XUtilities.XWeb.ExecuteHttp ... Exception: " + x );
            }
        }
        public static void SetResponseStatus( HttpStatusCode status )
        {
            if ( WebOperationContext.Current == null ) return;
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }

        public static string GetCallerIP()
        {
            try
            {
                var ipEntry = Dns.GetHostEntry( Dns.GetHostName() );
                if ( ipEntry == null ) return "";

                var ipAddressList = ipEntry.AddressList;
                if ( ipAddressList.Length == 0 ) return "";

                foreach ( var ipAddress in ipAddressList )
                {
                    if ( ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
                    {
                        return ipAddress.ToString();
                    }
                }

                return "";
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return "";
            }
        }
        public static string GetCallerIP( HttpContext httpContext )
        {
            try
            {
                string ip = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if ( !string.IsNullOrEmpty( ip ) )
                {
                    var addresses = ip.Split( ',' );
                    if ( addresses.Length != 0 ) return addresses[0];
                }

                return httpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public static string GetCallerIP( WebOperationContext webContext )
        {
            try
            {
                string ip = webContext.OutgoingResponse.Headers["HTTP_X_FORWARDED_FOR"];
                if ( !string.IsNullOrEmpty( ip ) )
                {
                    var addresses = ip.Split( ',' );
                    if ( addresses.Length != 0 ) return addresses[0];
                }

                return webContext.OutgoingResponse.Headers["REMOTE_ADDR"];
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public static string GetCallerMachineName()
        {
            return "";

            try
            {
                //return Dns.GetHostEntry( Dns.GetHostName() ).HostName;
                //or
                return ( Dns.GetHostEntry( HttpContext.Current.Request.ServerVariables["remote_addr"] ).HostName );   // TODO : takes long time in some networks, look into it ...              
            }
            catch ( Exception x )
            {
                NLogger.Error( "Exception : " + x );
                return "";
            }
        }

        public static string GetVirtualPath()
        {
            string path = HttpContext.Current.Request.RawUrl;
            path = path.Substring( 0 , path.IndexOf( "?" ) );
            path = path.Substring( path.LastIndexOf( "/" ) + 1 );
            return path;
        }

        public static string NormalizeHeaderString( string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return value;

            var result = value;
            result = result.Replace( ',' , '_' );
            result = result.Replace( ' ' , '_' );

            return result;
        }

        #region Query String Handles

        public static string GetQueryString( string url )
        {
            int index = url.IndexOf( "?" ) + 1;
            return url.Substring( index );
        }

        public static string EncryptQueryString( string url )
        {
            var encryptedURL = url;
            try
            {
                if ( url.IndexOf( "?" ) == -1 ) return url;
                var queryStringIndex = url.IndexOf( "?" ) + 1;
                var encryptQueryString = XCrypto.EncryptToRijndael( GetQueryString( url ) );
                var pageURL = url.Substring( 0 , queryStringIndex );
                /*HttpUtility.UrlEncode() to query string after encrypt to solve the issue of convert '/' to '%2f' after post back 
             * based on the following article solution
             *http://forums.iis.net/t/1181612.aspx */
                encryptedURL = pageURL + HttpUtility.UrlEncode( encryptQueryString );
            }
            catch ( Exception ex )
            {
                NLogger.Error( "Exception: " + ex );

            }
            return encryptedURL;
        }

        public static string EncryptQueryString( HttpRequest request )
        {
            return EncryptQueryString( request.RawUrl );
        }

        public static string DecryptQueryString( string url )
        {
            var decryptedUrl = string.Empty;
            try
            {
                /* HttpUtility.UrlDecode() to url before decrypt to solve the issue of convert '/' to '%2f' after post back 
                 * based on the following article solution
                 * http://forums.iis.net/t/1181612.aspx */
                url = HttpUtility.UrlDecode( url );
                if ( url.IndexOf( "?" ) == -1 ) return url;
                var queryStringIndex = url.IndexOf( "?" ) + 1;
                var pageURL = url.Substring( 0 , queryStringIndex );
                var decryptedQueryString = XCrypto.DecryptFromRijndael( GetQueryString( url ) );
                decryptedUrl = pageURL + decryptedQueryString;

            }
            catch ( Exception ex )
            {
                NLogger.Error( "Exception: " + ex );

            }
            return decryptedUrl;
        }

        public static string DecryptQueryString( HttpRequest request )
        {
            return DecryptQueryString( request.RawUrl );
        }

        public static Dictionary<string , string> ExtractQueryStringParameters( string url )
        {
            try
            {
                var queryStringParameters = new Dictionary<string , string>();
                var queryStringPart = GetQueryString( url );
                var parameters = queryStringPart.Split( '&' );
                foreach ( var queryStringParameter in parameters )
                {
                    var parameterKeyValue = queryStringParameter.Split( '=' );
                    if ( parameterKeyValue.Length == 2 )
                        queryStringParameters.Add( parameterKeyValue[0].ToLower() , parameterKeyValue[1] );
                }
                return queryStringParameters;
            }
            catch ( Exception ex )
            {
                NLogger.Error( "Exception: " + ex );
                return null;
            }
        }
        #endregion
        #region Helpers

        private static bool IsHttpRequestEncodingAccepted( string encoding )
        {
            return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains( encoding );
        }
        private static void SetHttpResponseEncoding( string encoding )
        {
            HttpContext.Current.Response.AppendHeader( "Content-encoding" , encoding );
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;
        }
        private static string RemoveWhitespaceFromHtml( string html )
        {
            html = RegexBetweenTags.Replace( html , ">" );
            html = RegexLineBreaks.Replace( html , string.Empty );

            return html;
        }
        private static string RemoveWhitespaceFromCss( string body )
        {
            body = body.Replace( "  " , " " );
            body = body.Replace( Environment.NewLine , String.Empty );
            body = body.Replace( "\t" , string.Empty );
            body = body.Replace( " {" , "{" );
            body = body.Replace( " :" , ":" );
            body = body.Replace( ": " , ":" );
            body = body.Replace( ", " , "," );
            body = body.Replace( "; " , ";" );
            body = body.Replace( ";}" , "}" );

            // sometimes found when retrieving CSS remotely
            body = body.Replace( @"?" , string.Empty );

            //body = Regex.Replace(body, @"/\*[^\*]*\*+([^/\*]*\*+)*/", "$1");
            body = Regex.Replace( body , @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])" , String.Empty );

            //Remove comments from CSS
            body = Regex.Replace( body , @"/\*[\d\D]*?\*/" , string.Empty );

            return body;
        }
        private static string RemoveWhitespaceFromJavaScript( string body )
        {
            string[] lines = body.Split( new[] { Environment.NewLine } , StringSplitOptions.RemoveEmptyEntries );
            StringBuilder emptyLines = new StringBuilder();
            foreach ( string line in lines )
            {
                string s = line.Trim();
                if ( s.Length > 0 && !s.StartsWith( "//" ) ) emptyLines.AppendLine( s.Trim() );
            }

            body = emptyLines.ToString();

            // remove C styles comments
            body = Regex.Replace( body , "/\\*.*?\\*/" , string.Empty , RegexOptions.Compiled | RegexOptions.Singleline );
            // trim left
            body = Regex.Replace( body , "^\\s*" , string.Empty , RegexOptions.Compiled | RegexOptions.Multiline );
            // trim right
            body = Regex.Replace( body , "\\s*[\\r\\n]" , "\r\n" , RegexOptions.Compiled | RegexOptions.ECMAScript );
            // remove whitespace beside of left curly braced
            body = Regex.Replace( body , "\\s*{\\s*" , "{" , RegexOptions.Compiled | RegexOptions.ECMAScript );
            // remove whitespace beside of coma
            body = Regex.Replace( body , "\\s*,\\s*" , "," , RegexOptions.Compiled | RegexOptions.ECMAScript );
            // remove whitespace beside of semicolon
            body = Regex.Replace( body , "\\s*;\\s*" , ";" , RegexOptions.Compiled | RegexOptions.ECMAScript );
            // remove newline after keywords
            body = Regex.Replace( body , "\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)" , " " , RegexOptions.Compiled | RegexOptions.ECMAScript );

            return body;
        }

        private static Stream ExecuteHttp( string command , string url , string contentType , NameValueCollection headers , object body )
        {
            if ( string.IsNullOrEmpty( url ) ) return null;

            var request = ( HttpWebRequest ) WebRequest.Create( url );
            if ( !string.IsNullOrEmpty( command ) ) request.Method = command;
            if ( !string.IsNullOrEmpty( contentType ) ) request.ContentType = contentType;
            if ( headers != null && headers.Count != 0 ) request.Headers.Add( headers );

            if ( body != null )
            {
                using ( var writer = new StreamWriter( request.GetRequestStream() ) )
                {
                    writer.Write( body );
                }
            }

            return request.GetResponse().GetResponseStream();
        }
        private static void CopyStream( Stream source , Stream target )
        {
            byte[] buffer = new byte[StreamBuffer];
            int bytesRead;

            do
            {
                bytesRead = source.Read( buffer , 0 , buffer.Length );
                target.Write( buffer , 0 , bytesRead );
            }
            while ( bytesRead > 0 );
        }
        private static void PermanentRedirect( string url , HttpContext context )
        {
            context.Response.Clear();
            context.Response.StatusCode = 301;
            context.Response.AppendHeader( "location" , url );
            context.Response.End();
        }

        #endregion
        #region Enums

        public enum FileContentType { HTML, CSS, JavaScript, }
        public static class StreamContentType
        {
            public const string Plain = "text/plain";
            public const string Html = "text/html";
            public const string Xml = "text/xml";
            public const string Atom = "application/atom+xml";
            public const string AtomEntry = "application/atom+xml;type=entry";
            public const string AtomServiceDocument = "application/atomsvc+xml";
        }
        public static class HttpCommand
        {
            public const string Get = "GET";
            public const string Post = "POST";
            public const string Update = "PUT";
            public const string Delete = "DELETE";
        }
        public static class HttpEncodingType
        {
            public const string GZIP = "gzip";
            public const string Deflate = "deflate";
        }

        #endregion
    }
}
