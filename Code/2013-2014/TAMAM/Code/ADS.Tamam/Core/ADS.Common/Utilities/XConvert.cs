using System;
using System.Collections.Generic;

namespace ADS.Common.Utilities
{
    public static class XConvert
    {
        #region Time

        /// <summary>
        /// hours -> hh:mm
        /// </summary>
        public static string FromHourseToHours( double hours )
        {
            if ( hours == 0 ) return "";

            return ( ( int ) hours ) + ":" + ( System.Math.Round( ( ( Math.Abs( hours ) * 60 ) % 60 ) ) ).ToString( "00" ).Replace( "0:00" , "" );
        }
        /// <summary>
        /// hours -> hh:mm
        /// </summary>
        public static string FromHourseToHours( int hours )
        {
            if ( hours == 0 ) return "";

            return ( hours ) + ":" + ( ( Math.Abs( hours ) * 60 ) % 60 ).ToString( "00" ).Replace( "0:00" , "" );
        }
        /// <summary>
        /// minutes -> hh:mm
        /// </summary>
        public static string FromMinutesToHours( int minutes )
        {
            return minutes == 0 ? "" : ( ( minutes / 60 ) + ":" + ( Math.Abs( minutes ) % 60 ).ToString( "00" ) );
        }
        /// <summary>
        /// minutes -> hh:mm
        /// </summary>
        public static string FromMinutesToHours( double minutes )
        {
            return minutes == 0 ? "" : ( ( minutes / 60 ).ToString( "00" ) + ":" + ( Math.Abs( minutes ) % 60 ).ToString( "00" ) );
        }
        
        #endregion
        #region Int

        /// <summary>
        /// string -> int : return 0 in case of invalid int
        /// </summary>
        public static int ToInt( string value )
        {
            int n = 0;
            bool state = int.TryParse( value , out n );

            return n;
        }
        
        #endregion
        #region Double

        /// <summary>
        /// string -> double : return 0 in case of invalid double
        /// </summary>
        public static double ToDouble( string value )
        {
            double n = 0;
            bool state = double.TryParse( value , out n );

            return n;
        }

        #endregion
        #region List<Guid>

        /// <summary>
        /// Try Parse String of Guid values into List<Guid>, like Department Ids
        /// </summary>
        /// <param name="s"></param>
        /// <returns>List<Guid></returns>
        public static List<Guid> ToGuidList(this string s)
        {
            if ( string.IsNullOrWhiteSpace( s ) ) return null;

            var strings = s.Split(',');

            if ( strings.Length == 0 ) return null;

            var list = new List<Guid>();
            for (var i = 0; i < strings.Length; i++)
            {
                Guid temp;
                Guid.TryParse(strings[i], out temp);
                list.Add(temp);
            }
            return list;
        } 
        #endregion
    }
}
