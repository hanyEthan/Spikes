using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RecurrenceGenerator;

namespace ADS.Common.Utilities
{
    public class DatesSchedule
    {
        #region nested.

        public class Pattern
        {
            public class MonthlyPattern
            {
                public int? DayNumber { get; set; }
                public Enums.DayOrders? DayOrder { get; set; }
                public Enums.WeekDayNames? WeekDayName { get; set; }
                public Enums.MonthlyPatternType? MonthlyPatternType { get; set; }
            }
            public class WeeklyPattern
            {
                public bool Sunday { get; set; }
                public bool Monday { get; set; }
                public bool Tuesday { get; set; }
                public bool Wednesday { get; set; }
                public bool Thursday { get; set; }
                public bool Friday { get; set; }
                public bool Saturday { get; set; }

                #region Methods

                public int GetSelectedDaysCount()
                {
                    var count = 0;
                    if ( Sunday ) ++count;
                    if ( Monday ) ++count;
                    if ( Tuesday ) ++count;
                    if ( Wednesday ) ++count;
                    if ( Thursday ) ++count;
                    if ( Friday ) ++count;
                    if ( Saturday ) ++count;
                    return count;
                }

                #endregion
            }
            public class YearlyPattern
            {
                public int? DayNumber { get; set; }
                public Enums.DayOrders? DayOrder { get; set; }
                public Enums.WeekDayNames? WeekDayName { get; set; }
                public Enums.Months? Month { get; set; }

                public Enums.YearlyPatternType? YearlyPatternType { get; set; }
            }

            public int OccurEvery { get; set; }

            // End Pattern
            public int? Occurrences { get; set; }
            public bool? NeverEnd { get; set; }
            public Enums.RecurrenceType RecurrenceType { get; set; }


            public WeeklyPattern Weekly { get; set; }
            public MonthlyPattern Monthly { get; set; }
            public YearlyPattern Yearly { get; set; }
        }
        public class Enums
        {
            public enum DayOrders
            {
                NotSet = -1,
                First = 0,
                Second = 1,
                Third = 2,
                Fourth = 3,
                Last = 4,
            }
            public enum MonthlyPatternType
            {
                DayOfMonth = 1,
                AdjustedDay = 2
            }
            public enum Months
            {
                January = 1,
                February = 2,
                March = 3,
                April = 4,
                May = 5,
                June = 6,
                July = 7,
                August = 8,
                September = 9,
                October = 10,
                November = 11,
                December = 12
            }
            public enum RecurrenceType
            {
                Daily = 1,
                Weekly = 2,
                Monthly = 3,
                Yearly = 4
            }
            public enum WeekDayNames
            {
                Sunday = 1,
                Monday = 2,
                Tuesday = 3,
                Wednesday = 4,
                Thursday = 5,
                Friday = 6,
                Saturday = 7
            }
            public enum YearlyPatternType
            {
                DayOfYear = 1,
                AdjustedDay = 2
            }
        }
        private class Mapper
        {
            public static void Map( Pattern.WeeklyPattern from , out SelectedDayOfWeekValues to )
            {
                to = new SelectedDayOfWeekValues()
                {
                    Sunday = from.Sunday ,
                    Monday = from.Monday ,
                    Tuesday = from.Tuesday ,
                    Wednesday = from.Wednesday ,
                    Thursday = from.Thursday ,
                    Friday = from.Friday ,
                    Saturday = from.Saturday
                };
            }

            public static void Map( Enums.DayOrders from , out MonthlySpecificDatePartOne to )
            {
                to = ( MonthlySpecificDatePartOne ) ( ( int ) from );
            }
            public static void Map( Enums.WeekDayNames from , out MonthlySpecificDatePartTwo to )
            {
                to = ( MonthlySpecificDatePartTwo ) ( ( int ) from );
            }

            public static void Map( Enums.DayOrders from , out YearlySpecificDatePartOne to )
            {
                to = ( YearlySpecificDatePartOne ) ( ( int ) from );
            }
            public static void Map( Enums.WeekDayNames from , out YearlySpecificDatePartTwo to )
            {
                to = ( YearlySpecificDatePartTwo ) ( ( int ) from );
            }
            public static void Map( Enums.Months from , out YearlySpecificDatePartThree to )
            {
                to = ( YearlySpecificDatePartThree ) ( ( int ) from );
            }
        }

        #endregion
        #region helpers.

        private static List<DateTime> BreakDownDailyPattern( DateTime start , DateTime? end , Pattern pattern )
        {
            if ( end.HasValue )
            {
                var dailySettings = new DailyRecurrenceSettings( start , end.Value );
                return dailySettings.GetValues( pattern.OccurEvery ).Values;
            }
            else if ( pattern.Occurrences.HasValue )
            {
                var dailySettings = new DailyRecurrenceSettings( start , pattern.Occurrences.Value );
                return dailySettings.GetValues( pattern.OccurEvery ).Values;
            }
            else if ( pattern.NeverEnd.HasValue && pattern.NeverEnd.Value )
            {
                return null;
                // No End
            }

            return null;
        }
        private static List<DateTime> BreakDownWeeklyPattern( DateTime start , DateTime? end , Pattern pattern )
        {
            SelectedDayOfWeekValues weekValues;
            Mapper.Map( pattern.Weekly , out weekValues );

            if ( end.HasValue )
            {
                var weeklySettings = new WeeklyRecurrenceSettings( start , end.Value );
                return weeklySettings.GetValues( pattern.OccurEvery , weekValues ).Values;
            }
            else if ( pattern.Occurrences.HasValue )
            {
                var weekValuesCount = weekValues.GetSelectedDaysCount();
                // count of selected days per week. (Sun, Mon)
                var realOccurance = pattern.Occurrences.Value * weekValuesCount;
                // Total generated Days = Number of selected days per week * week occurence

                var weeklySettings = new WeeklyRecurrenceSettings( start , realOccurance );
                return weeklySettings.GetValues( pattern.OccurEvery , weekValues ).Values;
            }
            else if ( pattern.NeverEnd.HasValue && pattern.NeverEnd.Value )
            {
                return null;
                // No End
            }

            return null;
        }
        private static List<DateTime> BreakDownMonthlyPattern( DateTime start , DateTime? end , Pattern pattern )
        {
            var dayOrder = MonthlySpecificDatePartOne.NotSet;
            var WeeklyDay = MonthlySpecificDatePartTwo.NotSet;

            if ( pattern.Monthly.DayOrder.HasValue ) Mapper.Map( pattern.Monthly.DayOrder.Value , out dayOrder );
            if ( pattern.Monthly.WeekDayName.HasValue ) Mapper.Map( pattern.Monthly.WeekDayName.Value , out WeeklyDay );

            if ( end.HasValue )
            {
                var monthlySettings = new MonthlyRecurrenceSettings( start , end.Value );

                switch ( pattern.Monthly.MonthlyPatternType )
                {
                    case Enums.MonthlyPatternType.DayOfMonth:
                        return monthlySettings.GetValues( pattern.Monthly.DayNumber.Value , pattern.OccurEvery ).Values;

                    case Enums.MonthlyPatternType.AdjustedDay:
                        return monthlySettings.GetValues( dayOrder , WeeklyDay , pattern.OccurEvery ).Values;
                }
            }
            else if ( pattern.Occurrences.HasValue )
            {
                var monthlySettings = new MonthlyRecurrenceSettings( start , pattern.Occurrences.Value );

                switch ( pattern.Monthly.MonthlyPatternType )
                {
                    case Enums.MonthlyPatternType.DayOfMonth:
                        return monthlySettings.GetValues( pattern.Monthly.DayNumber.Value , pattern.OccurEvery ).Values;

                    case Enums.MonthlyPatternType.AdjustedDay:
                        return monthlySettings.GetValues( dayOrder , WeeklyDay , pattern.OccurEvery ).Values;
                }
            }
            else if ( pattern.NeverEnd.HasValue && pattern.NeverEnd.Value )
            {
                return null;
                // No End
            }

            return null;
        }
        private static List<DateTime> BreakDownYearlyPattern( DateTime start , DateTime? end , Pattern pattern )
        {
            var dayOrder = YearlySpecificDatePartOne.NotSet;
            var WeeklyDay = YearlySpecificDatePartTwo.NotSet;
            var months = YearlySpecificDatePartThree.NotSet;

            if ( pattern.Yearly.DayOrder.HasValue ) Mapper.Map( pattern.Yearly.DayOrder.Value , out dayOrder );
            if ( pattern.Yearly.WeekDayName.HasValue ) Mapper.Map( pattern.Yearly.WeekDayName.Value , out WeeklyDay );
            if ( pattern.Yearly.Month.HasValue ) Mapper.Map( pattern.Yearly.Month.Value , out months );

            if ( end.HasValue )
            {
                var yearlySettings = new YearlyRecurrenceSettings( start , end.Value );

                switch ( pattern.Yearly.YearlyPatternType )
                {
                    case Enums.YearlyPatternType.DayOfYear:
                        return yearlySettings.GetValues( pattern.Yearly.DayNumber.Value , ( int ) pattern.Yearly.Month.Value ).Values;

                    case Enums.YearlyPatternType.AdjustedDay:
                        return yearlySettings.GetValues( dayOrder , WeeklyDay , months ).Values;
                }
            }
            else if ( pattern.Occurrences.HasValue )
            {
                var yearlySettings = new YearlyRecurrenceSettings( start , pattern.Occurrences.Value );

                switch ( pattern.Yearly.YearlyPatternType )
                {
                    case Enums.YearlyPatternType.DayOfYear:
                        return yearlySettings.GetValues( pattern.Yearly.DayNumber.Value , ( int ) pattern.Yearly.Month.Value ).Values;
                        break;

                    case Enums.YearlyPatternType.AdjustedDay:
                        return yearlySettings.GetValues( dayOrder , WeeklyDay , months ).Values;
                        break;
                }
            }
            else if ( pattern.NeverEnd.HasValue && pattern.NeverEnd.Value )
            {
                return null;
                // No End
            }

            return null;
        }

        #endregion
        #region publics.

        public static List<DateTime> GetDates( DateTime start , DateTime? end , Pattern pattern )
        {
            try
            {
                switch ( pattern.RecurrenceType )
                {
                    case Enums.RecurrenceType.Daily: return BreakDownDailyPattern( start , end , pattern );
                    case Enums.RecurrenceType.Weekly: return BreakDownWeeklyPattern( start , end , pattern );
                    case Enums.RecurrenceType.Monthly: return BreakDownMonthlyPattern( start , end , pattern );
                    default: return BreakDownYearlyPattern( start , end , pattern );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "XDates.BreakDownPattern ... Exception : " + x );
                return null;
            }
        }
        public static DateTime? GetNextDate( DateTime start , DateTime? end , DateTime current , Pattern pattern )
        {
            var dates = GetDates( start , end , pattern );
            var nextDates = dates.Where( x => x > current ).ToList();

            return nextDates.Count > 0 ? ( DateTime? ) nextDates[0] : null;
        }

        #endregion
    }
}
