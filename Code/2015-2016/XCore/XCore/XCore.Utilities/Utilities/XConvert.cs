using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Utilities.Utilities
{
    public static class XConvert
    {
        #region Int

        public static int ToInt( string value )
        {
            return int.TryParse( value , out int n ) ? n : 0;
        }
        public static int? ToIntNullable( string value )
        {
            return int.TryParse( value , out int n ) ? n : ( int? ) null;
        }

        #endregion
        #region Double

        public static double ToDouble( string value )
        {
            return double.TryParse( value , out double n ) ? n : 0;
        }
        public static double? ToDoubleNullable( string value )
        {
            return double.TryParse( value , out double n ) ? n : ( double? ) null;
        }

        #endregion
        #region Decimal

        public static decimal ToDecimal( string value )
        {
            return decimal.TryParse( value , out decimal n ) ? n : 0;
        }
        public static decimal? ToDecimalNullable( string value )
        {
            return decimal.TryParse( value , out decimal n ) ? n : ( decimal? ) null;
        }

        #endregion
        #region Bool

        public static bool ToBool( string value )
        {
            return bool.TryParse( value , out bool n ) ? n : false;
        }
        public static bool? ToBoolNullable( string value )
        {
            return bool.TryParse( value , out bool n ) ? n : ( bool? ) null;
        }

        #endregion
        #region Guid

        /// <summary>
        /// Try Parse String of Guid values into List<Guid>
        /// </summary>
        public static List<Guid> ToGuidList( this string s )
        {
            if ( string.IsNullOrWhiteSpace( s ) ) return null;
            var strings = s.Split( ',' );
            if ( strings.Length == 0 ) return null;

            var list = new List<Guid>();
            for ( var i = 0 ; i < strings.Length ; i++ )
            {
                Guid.TryParse( strings[i] , out Guid temp );
                list.Add( temp );
            }
            return list;
        }

        #endregion
    }
}
