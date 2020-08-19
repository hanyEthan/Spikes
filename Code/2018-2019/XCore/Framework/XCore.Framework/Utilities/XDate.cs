using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace XCore.Framework.Utilities
{
    public static class XDate
    {
        #region publics.

        public static string Convert( DateTime date , CalendarType type )
        {
            try
            {
                switch ( type )
                {
                    case CalendarType.Hijri: return ConvertToHijri( date );
                    case CalendarType.Persian: return ConvertToPersianLegacy( date );
                    default: return ConvertToGregorian( date );
                }
            }
            catch
            {
                return null;
            }
        }
        public static string Convert( DateTime date , string format , CalendarType type )
        {
            try
            {
                switch ( type )
                {
                    case CalendarType.Hijri: return ConvertToHijri( date , format );
                    case CalendarType.Persian: return ConvertToPersianLegacy( date , format );
                    default: return ConvertToGregorian( date , format );
                }
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? Parse( string date , CalendarType type )
        {
            try
            {
                switch ( type )
                {
                    case CalendarType.Hijri: return ParseHijri( date );
                    case CalendarType.Persian: return ParsePersianLegacy( date );
                    default: return ParseGregorian( date );
                }
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? Parse( string date , string format , CalendarType type )
        {
            try
            {
                switch ( type )
                {
                    case CalendarType.Hijri: return ParseHijri( date , format );
                    case CalendarType.Persian: return ParsePersianLegacy( date , format );
                    default: return ParseGregorian( date , format );
                }
            }
            catch
            {
                return null;
            }
        }

        public static void Swap( ref DateTime A , ref DateTime B )
        {
            DateTime x;
            x = A;
            A = B;
            B = x;
        }
        public static void Swap( ref DateTime? A , ref DateTime? B )
        {
            DateTime? x;
            x = A;
            A = B;
            B = x;
        }

        public static DateTime? Max( DateTime? A , DateTime? B )
        {
            return A == null || B == null
                   ? null
                   : ( DateTime? ) new DateTime( Math.Max( A.Value.Ticks , B.Value.Ticks ) );
        }
        public static DateTime? Min( DateTime? A , DateTime? B )
        {
            return A == null || B == null
                   ? null
                   : ( DateTime? ) new DateTime( Math.Min( A.Value.Ticks , B.Value.Ticks ) );
        }
        public static TimeSpan? Max( TimeSpan? A , TimeSpan? B )
        {
            return A == null || B == null
                   ? null
                   : ( TimeSpan? ) new TimeSpan( Math.Max( A.Value.Ticks , B.Value.Ticks ) );
        }
        public static TimeSpan? Min( TimeSpan? A , TimeSpan? B )
        {
            return A == null || B == null
                   ? null
                   : ( TimeSpan? ) new TimeSpan( Math.Min( A.Value.Ticks , B.Value.Ticks ) );
        }

        public static bool IsMax( DateTime A )
        {
            return A == DateTime.MaxValue.Date;
        }
        public static bool IsMax( DateTime? A )
        {
            return A.HasValue && A.Value == DateTime.MaxValue.Date;
        }
        public static bool IsMin( DateTime A )
        {
            return A == DateTime.MinValue.Date;
        }
        public static bool IsMin( DateTime? A )
        {
            return A.HasValue && A.Value == DateTime.MinValue.Date;
        }

        public static List<DateTime> Sequence( DateTime start , DateTime end )
        {
            return Sequence( start , end , false );
        }
        public static List<DateTime> Sequence( DateTime start , DateTime end , bool IsDayEndTimes )
        {
            if ( start > end ) Swap( ref start , ref end );

            var range = Enumerable.Range( 0 , ( int ) ( ( ( end.Date ).AddDays( -1 ) - start.Date ).TotalDays ) + 1 );

            return IsDayEndTimes ? range.Select( n => ( start.Date.AddDays( n ) ).AddDays( 1 ).AddSeconds( -1 ) ).ToList()
                                 : range.Select( n => ( start.Date.AddDays( n ) ) ).ToList();
        }

        public static DateTime WeekStart()
        {
            return WeekStart( DateTime.Today );
        }
        public static DateTime WeekStart( DateTime date )
        {
            if ( date.DayOfWeek == WeekStartDay ) return date;

            var offset = date.DayOfWeek - WeekStartDay;
            offset += offset < 0 ? 7 : 0;
            return date.AddDays( offset * -1 ).Date;
        }
        public static DateTime WeekEnd()
        {
            return WeekEnd( DateTime.Today );
        }
        public static DateTime WeekEnd( DateTime date )
        {
            return WeekStart( date ).AddDays( 6 ).Date;
        }

        #endregion
        #region extensions.

        public static DateTime? AddDays( this DateTime? date , int days )
        {
            return date.HasValue ? ( DateTime? ) date.Value.AddDays( days ) : null;
        }

        #endregion

        #region props.

        private static DateTimeFormatInfo HijriDTFI;
        private static DateTimeFormatInfo GregorianDTFI;
        private static DateTimeFormatInfo PersianDTFI;

        private static PersianCalendar PersianCalendar;

        private static string[] allFormats =
        {
            "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy",
            "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy",
            "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy"
        };

        private static DayOfWeek WeekStartDay { get; set; }
        private static DayOfWeek WeekEndDay { get { return WeekStartDay == DayOfWeek.Sunday ? DayOfWeek.Saturday : WeekStartDay - 1; } }

        #endregion
        #region nested.

        public enum CalendarType
        {
            Gregorian,
            Hijri,
            Persian,
        }

        #endregion
        #region cst.

        static XDate()
        {
            try
            {
                InitializeCultures();
            }
            catch
            {
            }
        }

        #endregion
        #region helpers.

        private static void InitializeCultures()
        {
            HijriDTFI = new CultureInfo( "ar-SA" , false ).DateTimeFormat;
            HijriDTFI.Calendar = new HijriCalendar();
            HijriDTFI.ShortDatePattern = "dd/MM/yyyy";

            GregorianDTFI = new CultureInfo( "en-US" , false ).DateTimeFormat;
            GregorianDTFI.Calendar = new GregorianCalendar();
            //GregorianDTFI.ShortDatePattern = "dd/MM/yyyy";

            PersianCalendar = new PersianCalendar();
            PersianDTFI = new CultureInfo( "fa-IR" , false ).DateTimeFormat;
            PersianDTFI.Calendar = PersianCalendar;
            PersianDTFI.ShortDatePattern = "dd/MM/yyyy";
        }

        private static string ConvertToHijri( DateTime date )
        {
            try
            {
                return date.ToString( HijriDTFI );
            }
            catch
            {
                return null;
            }
        }
        private static string ConvertToHijri( DateTime date , string format )
        {
            try
            {
                return date.ToString( format , HijriDTFI );
            }
            catch
            {
                return null;
            }
        }
        private static string ConvertToGregorian( DateTime date )
        {
            try
            {
                return date.ToString( GregorianDTFI );
            }
            catch
            {
                return null;
            }
        }
        private static string ConvertToGregorian( DateTime date , string format )
        {
            try
            {
                return date.ToString( format , GregorianDTFI );
            }
            catch
            {
                return null;
            }
        }
        private static string ConvertToPersian( DateTime date )
        {
            try
            {
                return date.ToString( PersianDTFI );
            }
            catch
            {
                return null;
            }
        }
        private static string ConvertToPersian( DateTime date , string format )
        {
            return ConvertToPersian( date );
        }
        private static string ConvertToPersianLegacy( DateTime date )
        {
            try
            {
                return string.Format( "{0}/{1}/{2}" , PersianCalendar.GetDayOfMonth( date ) , PersianCalendar.GetMonth( date ) , PersianCalendar.GetYear( date ) );
            }
            catch
            {
                return null;
            }
        }
        private static string ConvertToPersianLegacy( DateTime date , string format )
        {
            try
            {
                return date.ToString( format , PersianDTFI );
            }
            catch
            {
                return null;
            }
        }

        private static DateTime? ParseHijri( string date )
        {
            try
            {
                DateTime model;
                return DateTime.TryParseExact( date , allFormats , HijriDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ? ( DateTime? ) model : null;
            }
            catch
            {
                return null;
            }
        }
        private static DateTime? ParseHijri( string date , string format )
        {
            try
            {
                DateTime model;
                return DateTime.TryParseExact( date , format , HijriDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ? ( DateTime? ) model : null;
            }
            catch
            {
                return null;
            }
        }
        private static DateTime? ParseGregorian( string date )
        {
            try
            {
                DateTime model;
                return DateTime.TryParseExact( date , allFormats , GregorianDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ? ( DateTime? ) model : null;
            }
            catch
            {
                return null;
            }
        }
        private static DateTime? ParseGregorian( string date , string format )
        {
            try
            {
                DateTime model;
                return DateTime.TryParseExact( date , format , GregorianDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ? ( DateTime? ) model : null;
            }
            catch
            {
                return null;
            }
        }
        private static DateTime? ParsePersian( string date )
        {
            try
            {
                DateTime model;
                return DateTime.TryParseExact( date , allFormats , PersianDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ? ( DateTime? ) model : null;
            }
            catch
            {
                return null;
            }
        }
        private static DateTime? ParsePersian( string date , string format )
        {
            try
            {
                DateTime model;
                return DateTime.TryParseExact( date , format , PersianDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ? ( DateTime? ) model : null;
            }
            catch
            {
                return null;
            }
        }
        private static DateTime? ParsePersianLegacy( string date )
        {
            try
            {
                DateTime model;
                if ( !DateTime.TryParseExact( date , allFormats , GregorianDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ) return null;

                return PersianCalendar.ToDateTime( model.Year , model.Month , model.Day , 0 , 0 , 0 , 0 );
            }
            catch
            {
                return null;
            }
        }
        private static DateTime? ParsePersianLegacy( string date , string format )
        {
            try
            {
                DateTime model;
                if ( !DateTime.TryParseExact( date , format , GregorianDTFI , DateTimeStyles.AllowWhiteSpaces , out model ) ) return null;

                return PersianCalendar.ToDateTime( model.Year , model.Month , model.Day , 0 , 0 , 0 , 0 );
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
