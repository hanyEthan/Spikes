using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ADS.Common.Utilities
{
    public static class XString
    {
        public static bool MatchPattern( string inputString , string pattern )
        {
            if ( inputString == null || pattern == null ) return false;

            Regex reg = new Regex( pattern );
            Match match = reg.Match( inputString.Trim() );
            if ( !match.Success ) return false;

            if ( match.Value != inputString ) return false;

            return true;
        }

        public static bool MatchPattern( string inputString , string pattern , string[] groups , out Dictionary<string , string> groupsValues )
        {
            groupsValues = new Dictionary<string , string>();
            if ( inputString == null || pattern == null ) return false;

            Regex reg = new Regex( pattern );
            Match match = reg.Match( inputString.Trim() );
            if ( !match.Success ) return false;

            if ( groups != null )
            {
                foreach ( string group in groups )
                {
                    groupsValues[group] = match.Groups[group].Value;
                }
            }

            return true;
        }

        public static string ConvertFromStream( Stream stream )
        {
            stream.Seek( 0 , SeekOrigin.Begin );

            byte[] buffer = new byte[( int ) stream.Length];
            stream.Read( buffer , 0 , ( int ) stream.Length );

            return ConvertFromUtf8( buffer );
        }
        public static string ConvertFromBase64( string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return null;

            byte[] bytes = Convert.FromBase64String( value );
            var encoding = new UTF8Encoding();
            return encoding.GetString( bytes );
        }
        public static string ConvertFromUtf8( byte[] value )
        {
            return Encoding.UTF8.GetString( value , 0 , value.Length );
        }
        public static string ConvertFromList(List<string> value , string seperator)
        {
            if ( value == null ) return null;

            var builder = new StringBuilder();

            foreach ( string item in value )
            {
                builder.Append( item );
                builder.Append( seperator );
            }

            string result = builder.ToString();
            return result.Remove(result.Length - seperator.Length );
        }

        public static string ConvertToBase64( string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return null;

            var encoder = new UTF8Encoding();
            return Convert.ToBase64String( encoder.GetBytes( value ) );
        }
        public static byte[] ConvertToUtf8( string value )
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes( value );
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool XContains( this string source , string filter , StringComparison comp )
        {
            return source.IndexOf( filter, comp ) >= 0;
        }
    }
}
