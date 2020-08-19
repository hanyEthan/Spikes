using System;

namespace XCore.Utilities.Utilities
{
    public static class XFormat
    {
        #region Time

        /// <summary>
        /// hours -> hh:mm
        /// </summary>
        public static string HoursToHours( double hours )
        {
            if ( hours == 0 ) return "";

            return ( ( int ) hours ) + ":" + ( System.Math.Round( ( ( Math.Abs( hours ) * 60 ) % 60 ) ) ).ToString( "00" ).Replace( "0:00" , "" );
        }
        /// <summary>
        /// hours -> hh:mm
        /// </summary>
        public static string HoursToHours( int hours )
        {
            if ( hours == 0 ) return "";

            return ( hours ) + ":" + ( ( Math.Abs( hours ) * 60 ) % 60 ).ToString( "00" ).Replace( "0:00" , "" );
        }
        /// <summary>
        /// minutes -> hh:mm
        /// </summary>
        public static string MinutesToHours( int minutes )
        {
            return minutes == 0 ? "" : ( ( minutes / 60 ) + ":" + ( Math.Abs( minutes ) % 60 ).ToString( "00" ) );
        }
        /// <summary>
        /// minutes -> hh:mm
        /// </summary>
        public static string MinutesToHours( double minutes )
        {
            return minutes == 0 ? "" : ( ( minutes / 60 ).ToString( "00" ) + ":" + ( Math.Abs( minutes ) % 60 ).ToString( "00" ) );
        }

        #endregion
    }
}
