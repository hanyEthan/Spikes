using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XCore.Framework.Utilities
{
    public static class XString
    {
        #region props.

        private static Random random = new Random();

        #endregion

        public static string RandomAlphanumeric( int length )
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string( Enumerable.Repeat( chars , length ).Select( s => s[random.Next( s.Length )] ).ToArray() );
        }
        public static bool MatchPattern( string inputString , string pattern )
        {
            if ( inputString == null || pattern == null ) return false;

            Regex reg = new Regex( pattern );
            Match match = reg.Match( inputString.Trim() );
            if ( !match.Success ) return false;

            if ( match.Value != inputString ) return false;

            return true;
        }

        public static string ConvertFromUtf8( byte[] value )
        {
            return Encoding.UTF8.GetString( value , 0 , value.Length );
        }
        public static byte[] ConvertToUtf8( string value )
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes( value );
        }
    }
}
