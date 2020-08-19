using System;
using System.Linq;
using System.Collections.Generic;
using XCore.Framework.Utilities;

namespace ADS.Common.Utilities
{
    public static class XIntervals
    {
        #region models

        public class DateRange
        {
            #region props

            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }

            #endregion
            #region cst

            public DateRange()
            {
            }
            public DateRange(DateTime? start, DateTime? end)
                : this()
            {
                this.Start = start;
                this.End = end;
            }

            #endregion
            #region publics

            public DateRange GetOrdered()
            {
                if (!Start.HasValue || !End.HasValue) return this;
                if (Start.HasValue && End.HasValue && End.Value > Start.Value) return this;

                return new DateRange(End, Start);
            }

            #endregion
        }
        public class TimeRange
        {
            #region props

            public TimeSpan? Start { get; set; }
            public TimeSpan? End { get; set; }

            public int? Minutes
            {
                get
                {
                    return Start.HasValue && End.HasValue ? (int?)Round((End.Value - Start.Value).TotalMinutes) : null;
                }
            }
            public double? Hours
            {
                get
                {
                    return Start.HasValue && End.HasValue ? (double?)((End.Value - Start.Value).TotalHours) : null;
                }
            }

            #endregion
            #region cst

            public TimeRange()
            {
            }
            public TimeRange(TimeSpan? start, TimeSpan? end)
                : this()
            {
                this.Start = start;
                this.End = end;
            }

            #endregion
            #region publics

            public TimeRange GetOrdered()
            {
                if (!Start.HasValue || !End.HasValue) return this;
                if (End.Value > Start.Value) return this;

                return new TimeRange(End, Start);
            }
            public bool IsPoint()
            {
                return Start.HasValue && End.HasValue && Start.Value == End.Value;
            }

            #endregion
            #region helpers

            private int Round(double value)
            {
                return (int)Math.Round(value, MidpointRounding.AwayFromZero);
            }

            #endregion
        }

        public interface ITimeRange
        {
            Guid Id { get; set; }
            DateTime? StartTime { get; set; }
            DateTime? EndTime { get; set; }
            bool SpansMultipleDays { get; }
        }

        #endregion

        public static void Swap(ref DateTime start, ref DateTime end)
        {
            DateTime x;
            x = start;
            start = end;
            end = x;
        }
        public static void Swap(ref DateTime? start, ref DateTime? end)
        {
            DateTime? x;
            x = start;
            start = end;
            end = x;
        }

        public static TimeRange Intersect(TimeRange A, TimeRange B)
        {
            if (!IsOverlapped(A, B)) return null;

            var i = new TimeRange();

            i.Start = A.Start >= B.Start ? A.Start : B.Start;   // max of starts
            i.End = A.End >= B.End ? B.End : A.End;   // min of ends

            return i;
        }
        public static DateRange Intersect(DateRange A, DateRange B)
        {
            if (!IsOverlapped(A, B)) return null;

            var i = new DateRange();

            i.Start = A.Start >= B.Start ? A.Start : B.Start;   // max of starts
            i.End = A.End >= B.End ? B.End : A.End;   // min of ends

            return i;
        }

        public static bool IsOverlapped(TimeRange A, TimeRange B)
        {
            A = A.GetOrdered();
            B = B.GetOrdered();

            return (!A.Start.HasValue || !B.End.HasValue || A.Start.Value <= B.End.Value) &&
                   (!A.End.HasValue || !B.Start.HasValue || A.End.Value >= B.Start.Value);
        }
        public static bool IsOverlapped(ITimeRange A, ITimeRange B)
        {
            if (A.SpansMultipleDays && !B.SpansMultipleDays)   // first is nightly / second is normal
            {
                return (!A.EndTime.HasValue || !B.StartTime.HasValue || A.EndTime.Value >= B.StartTime.Value) ||
                       (!A.StartTime.HasValue || !B.EndTime.HasValue || A.StartTime.Value <= B.EndTime.Value);
            }
            else if (B.SpansMultipleDays && !A.SpansMultipleDays)   // first is normal / second is nightly
            {
                return (!B.EndTime.HasValue || !A.StartTime.HasValue || B.EndTime.Value >= A.StartTime.Value) ||
                       (!B.StartTime.HasValue || !A.EndTime.HasValue || B.StartTime.Value <= A.EndTime.Value);
            }
            else if (!A.SpansMultipleDays && !B.SpansMultipleDays)   // first is normal / second is normal
            {
                return (!A.StartTime.HasValue || !B.EndTime.HasValue || A.StartTime.Value <= B.EndTime.Value) &&
                       (!A.EndTime.HasValue || !B.StartTime.HasValue || A.EndTime.Value >= B.StartTime.Value);
            }
            else   // first is nightly / second is nightly
            {
                return (!A.StartTime.HasValue || !B.EndTime.HasValue || A.StartTime.Value <= B.EndTime.Value) ||
                       (!A.EndTime.HasValue || !B.StartTime.HasValue || A.EndTime.Value >= B.StartTime.Value);
            }
        }
        public static bool IsOverlapped(DateRange A, DateRange B)
        {
            A = A.GetOrdered();
            B = B.GetOrdered();

            return (!A.Start.HasValue || !B.End.HasValue || A.Start.Value <= B.End.Value) &&
                   (!A.End.HasValue || !B.Start.HasValue || A.End.Value >= B.Start.Value);
        }

        public static bool IsSubset(TimeRange A, TimeRange B)
        {
            return IsSubset(A, B, true);
        }
        public static bool IsSubset(TimeRange A, TimeRange B, bool inclusive)
        {
            A = A.GetOrdered();
            B = B.GetOrdered();

            return (B.Start.HasValue || A.Start >= B.Start) &&
                   (B.End.HasValue || A.End <= B.End) &&
                   (inclusive || A.Start != B.Start || A.End != B.End);
        }

        public static ITimeRange Nearest(DateTime time, List<ITimeRange> list, bool isOut)
        {
            return Nearest(time, list, new TimeSpan(), new TimeSpan(), isOut);
        }
        public static ITimeRange Nearest(DateTime time, List<ITimeRange> list, TimeSpan marginBefore, TimeSpan marginAfter, bool isOut)
        {
            // ...
            if (list == null || list.Count == 0) return null;

            // ...
            var unlimitedTimeRange = list.Where(x => !x.StartTime.HasValue || !x.EndTime.HasValue).FirstOrDefault();
            if (unlimitedTimeRange != null) return unlimitedTimeRange;

            // ...
            var point = NormalizeTime(time);

            // ...
            var intersected = list.Where(x => !x.SpansMultipleDays ? (point >= NormalizeTime(x.StartTime) && point <= NormalizeTime(x.EndTime))
                                                                    : (point >= NormalizeTime(x.StartTime) || point <= NormalizeTime(x.EndTime))).FirstOrDefault();
            if (intersected != null) return intersected;

            // ...
            if (!isOut)
            {
                return list.Where(x => !x.SpansMultipleDays ? (point >= UpdateTimeWithMarginBefore(NormalizeTime(x.StartTime), marginBefore) && point <= NormalizeTime(x.EndTime))
                                                             : (point >= UpdateTimeWithMarginBefore(NormalizeTime(x.StartTime), marginBefore) || point <= NormalizeTime(x.EndTime)))

                           .OrderBy(x => GetDuration(x.StartTime.Value, point)).FirstOrDefault();
            }
            else
            {
                return list.Where(x => !x.SpansMultipleDays ? (point >= NormalizeTime(x.StartTime) && point <= UpdateTimeWithMarginAfter(NormalizeTime(x.EndTime), isOut ? (TimeSpan?)marginAfter : null))
                                                             : (point >= NormalizeTime(x.StartTime) || point <= UpdateTimeWithMarginAfter(NormalizeTime(x.EndTime), isOut ? (TimeSpan?)marginAfter : null)))

                           .OrderBy(x => GetDuration(x.StartTime.Value, point)).FirstOrDefault();
            }
        }
        public static ITimeRange Nearest(DateTime time, List<ITimeRange> list, Dictionary<Guid, TimeRange> margins, bool isOut)
        {
            // ...
            if (list == null || list.Count == 0) return null;

            // ...
            var unlimitedTimeRange = list.Where(x => !x.StartTime.HasValue || !x.EndTime.HasValue).FirstOrDefault();
            if (unlimitedTimeRange != null) return unlimitedTimeRange;

            // ...
            var point = NormalizeTime(time);

            // ...
            var intersected = list.Where(x => !x.SpansMultipleDays ? (point >= NormalizeTime(x.StartTime) && point <= NormalizeTime(x.EndTime))
                                                                    : (point >= NormalizeTime(x.StartTime) || point <= NormalizeTime(x.EndTime))).FirstOrDefault();
            if (intersected != null) return intersected;

            // ...
            if (!isOut)
            {
                return list.Where(x => !x.SpansMultipleDays ? (point >= UpdateTimeWithMarginBefore(NormalizeTime(x.StartTime), margins[x.Id].Start.Value) && point <= NormalizeTime(x.EndTime))
                                                             : (point >= UpdateTimeWithMarginBefore(NormalizeTime(x.StartTime), margins[x.Id].Start.Value) || point <= NormalizeTime(x.EndTime)))

                           .OrderBy(x => GetDuration(x.StartTime.Value, point)).FirstOrDefault();
            }
            else
            {
                return list.Where(x => !x.SpansMultipleDays ? (point >= NormalizeTime(x.StartTime) && point <= UpdateTimeWithMarginAfter(NormalizeTime(x.EndTime), isOut ? (TimeSpan?)margins[x.Id].End : null))
                                                             : (point >= NormalizeTime(x.StartTime) || point <= UpdateTimeWithMarginAfter(NormalizeTime(x.EndTime), isOut ? (TimeSpan?)margins[x.Id].End : null)))

                           .OrderBy(x => GetDuration(x.StartTime.Value, point)).FirstOrDefault();
            }
        }
        public static ITimeRange Nearest(DateTime time, List<ITimeRange> list, Dictionary<Guid, TimeRange> margins)
        {
            // ...
            if (list == null || list.Count == 0) return null;

            // ...
            var unlimitedTimeRange = list.Where(x => !x.StartTime.HasValue || !x.EndTime.HasValue).FirstOrDefault();
            if (unlimitedTimeRange != null) return unlimitedTimeRange;

            // ...
            var point = NormalizeTime(time);

            // ...
            var intersected = list.Where(x => !x.SpansMultipleDays ? (point >= NormalizeTime(x.StartTime) && point <= NormalizeTime(x.EndTime))
                                                                    : (point >= NormalizeTime(x.StartTime) || point <= NormalizeTime(x.EndTime))).FirstOrDefault();
            if (intersected != null) return intersected;

            // ...      
             
            return list.Where(x => !x.SpansMultipleDays ? (point >= UpdateTimeWithMarginBefore(NormalizeTime(x.StartTime), margins[x.Id].Start.Value) && point <= UpdateTimeWithMarginAfter(NormalizeTime(x.EndTime), (TimeSpan?)margins[x.Id].End))
                                                         : (point >= UpdateTimeWithMarginBefore(NormalizeTime(x.StartTime), margins[x.Id].Start.Value) || point <= UpdateTimeWithMarginAfter(NormalizeTime(x.EndTime), (TimeSpan?)margins[x.Id].End)))
                       .OrderBy(x => GetMinDuration(x.StartTime.Value,x.EndTime.Value, point)).FirstOrDefault();

        }

        public static DateRange Combine( ITimeRange A , ITimeRange B )
        {
            return IsOverlapped( A , B ) ? new DateRange( XDate.Min( A.StartTime , B.StartTime ) , XDate.Max(A.EndTime , B.EndTime) )
                                         : null;
        }

        #region Helpers

        private static DateTime NormalizeTime(DateTime dateTime)
        {
            return new DateTime().AddTicks(dateTime.TimeOfDay.Ticks);
        }
        private static DateTime NormalizeTime(DateTime? dateTime)
        {
            return new DateTime().AddTicks(dateTime.Value.TimeOfDay.Ticks);
        }

        private static DateTime UpdateTimeWithMarginBefore(DateTime time, TimeSpan? margin)
        {
            if (!margin.HasValue) return time;

            var marginEffective = new TimeSpan(0, XMath.Round(-margin.Value.TotalMinutes), 0);
            var marginOffset = marginEffective.TotalMinutes;
            var DayOffset = (new TimeSpan() - time.TimeOfDay).TotalMinutes;

            return time.AddMinutes((time.TimeOfDay.Add(marginEffective)).TotalMinutes < 0 ? DayOffset : marginOffset);
        }
        private static DateTime UpdateTimeWithMarginAfter(DateTime time, TimeSpan? margin)
        {
            var marginEffective = margin.HasValue ? new TimeSpan(0, XMath.Round(margin.Value.TotalMinutes), 0) : new TimeSpan();

            return time.AddMinutes(XMath.Round(marginEffective.TotalMinutes));
        }

        private static int GetDuration(DateTime from, DateTime to)
        {
            return XMath.Round(Math.Abs((from.TimeOfDay - to.TimeOfDay).TotalMinutes));
        }

        private static int GetMinDuration(DateTime start, DateTime end, DateTime to)
        {
          var Duration1 =  XMath.Round(Math.Abs((start.TimeOfDay - to.TimeOfDay).TotalMinutes));
          var Duration2 = XMath.Round(Math.Abs((end.TimeOfDay - to.TimeOfDay).TotalMinutes));
          return Math.Min(Duration1, Duration2);

        }
        #endregion
    }
}

#region TO REVIEW

//namespace ADS.Common.Utilities
//{
//    public class XDateRange : IEquatable<XDateRange>
//    {
//        #region Ctor
//        public XDateRange(DateTime start, DateTime end)
//        {
//            if (start.Date > end.Date)
//            {
//                throw new ArgumentException("Start Date should be less than or equal end date");
//            }

//            this.Start = start.Date;
//            this.End = end.Date;
//        }

//        #endregion

//        #region Props
//        public DateTime Start { get; private set; }

//        public DateTime End { get; private set; }
//        #endregion

//        #region Helpers

//        public bool Contains(DateTime date)
//        {
//            return (this.Start <= date) && (date <= this.End);
//        }

//        public bool Contains(XDateRange dateRange)
//        {
//            return (this.Start <= dateRange.Start) && (dateRange.End <= this.End);
//        }

//        public bool IsOverlaps(XDateRange dateRange)
//        {
//            return this.Start <= dateRange.End && dateRange.Start <= this.End;
//        }

//        public bool StartsWith(XDateRange dateRange)
//        {
//            return this.Start == dateRange.Start && this.End >= dateRange.End;
//        }

//        public bool EndsWith(XDateRange dateRange)
//        {
//            return this.End == dateRange.End && this.Start <= dateRange.Start;
//        }

//        public bool IsCompletelyBefore(XDateRange dateRange)
//        {
//            // This               [-----------]                        
//            // otherDaterange                            [------------]
//            return this.End < dateRange.Start;
//        }

//        public bool IsCompletelyAfter(XDateRange dateRange)
//        {
//            // This                                         [-----------]
//            // otherDaterange       [------------]                       
//            return this.Start > dateRange.End;
//        }

//        public List<XDateRange> SplitOn(List<XDateRange> dateRanges)
//        {
//            var splits = new List<XDateRange>();
//            foreach (var dateRange in dateRanges)
//            {
//                splits.AddRange(SplitOn(dateRange));
//            }
//            return splits;
//        }

//        public List<XDateRange> SplitOn(XDateRange dateRange)
//        {
//            var splits = new List<XDateRange>();

//            // This                                     [-----------]
//            // otherDaterange       [------------]                   
//            if (!this.IsOverlaps(dateRange))
//            {
//                splits.Add(this);
//                splits.Add(dateRange);
//                return splits;
//            }

//            // This                 [-----------------------------]
//            // otherDaterange       [-----------------------------]
//            if (this.Equals(dateRange))
//            {
//                splits.Add(this);
//                return splits;
//            }

//            // This                 [-----------------------------]
//            // otherDaterange             [----------------]       
//            if (this.Contains(dateRange) && !this.StartsWith(dateRange) && !this.EndsWith(dateRange))
//            {
//                splits.Add(new XDateRange(this.Start, dateRange.Start.AddDays(-1)));
//                splits.Add(new XDateRange(dateRange.Start, dateRange.End));
//                splits.Add(new XDateRange(dateRange.End.AddDays(1), this.End));
//                return splits;
//            }

//            // This                 [-----------------------------]
//            // otherDaterange       [----------------]             
//            if (this.StartsWith(dateRange))
//            {
//                splits.Add(new XDateRange(dateRange.Start, dateRange.End));
//                splits.Add(new XDateRange(dateRange.End.AddDays(1), this.End));
//                return splits;
//            }

//            // This                 [-----------------------------]
//            // otherDaterange                    [----------------]
//            if (this.EndsWith(dateRange))
//            {
//                splits.Add(new XDateRange(this.Start, dateRange.Start.AddDays(-1)));
//                splits.Add(new XDateRange(dateRange.Start, this.End));
//                return splits;
//            }

//            // This                        [--------------]        
//            // otherDaterange       [-----------------------------]
//            if (dateRange.Contains(this))
//            {
//                splits.Add(new XDateRange(this.Start, this.End));
//                return splits;
//            }

//            return splits;
//        }

//        public List<XDateRange> FindGaps(List<XDateRange> dateRanges)
//        {
//            var gaps = new List<XDateRange>();
//            foreach (var dateRange in dateRanges)
//            {
//                gaps.AddRange(FindGaps(dateRange));
//            }
//            return gaps;
//        }

//        public List<XDateRange> FindGaps(XDateRange dateRange)
//        {
//            var Gaps = new List<XDateRange>();

//            // This                 [-----------------------------]
//            // otherDaterange       [-----------------------------]
//            // Or
//            // This                 [-----------------------------]
//            // otherDaterange             [----------------]       
//            // or 
//            // This                 [-----------------------------]
//            // otherDaterange       [----------------]             
//            // Or
//            // This                 [-----------------------------]
//            // otherDaterange                    [----------------]
//            if (this.Contains(dateRange))
//            {
//                return Gaps;
//            }

//            // This                               xxxxxxx[-----------]
//            // otherDaterange       [------------]xxxxxxx             
//            if (this.IsCompletelyAfter(dateRange))
//            {
//                var gapStart = dateRange.End.AddDays(1);
//                var gapEnd = this.Start.AddDays(-1);

//                if (gapEnd >= gapStart)
//                {
//                    Gaps.Add(new XDateRange(gapStart, gapEnd));
//                    return Gaps;
//                }
//                else
//                {
//                    return Gaps;
//                }
//            }

//            // This                 [------------]xxxxxxx             
//            // otherDaterange                     xxxxxxx[-----------]
//            if (this.IsCompletelyBefore(dateRange))
//            {
//                var gapStart = this.End.AddDays(1);
//                var gapEnd = dateRange.Start.AddDays(-1);

//                if (gapEnd >= gapStart)
//                {
//                    Gaps.Add(new XDateRange(gapStart, gapEnd));
//                    return Gaps;
//                }
//                else
//                {
//                    return Gaps;
//                }
//            }

//            // This                 [--------------]xxxxxxxxxxxxxxx
//            // otherDaterange       [-----------------------------]
//            if (dateRange.StartsWith(this))
//            {
//                Gaps.Add(new XDateRange(this.End.AddDays(1), dateRange.End));
//                return Gaps;
//            }

//            // This                 xxxxxxxxxxxxxxx[--------------]
//            // otherDaterange       [-----------------------------]
//            if (dateRange.EndsWith(this))
//            {
//                Gaps.Add(new XDateRange(dateRange.Start, this.Start.AddDays(-1)));
//                return Gaps;
//            }

//            // This                 xxxxxxx[--------------]xxxxxxxx
//            // otherDaterange       [-----------------------------]
//            if (dateRange.Contains(this))
//            {
//                Gaps.Add(new XDateRange(dateRange.Start, this.Start.AddDays(-1)));
//                Gaps.Add(new XDateRange(this.End.AddDays(1), dateRange.End));
//                return Gaps;
//            }

//            return Gaps;
//        }

//        #endregion

//        #region Comparisons

//        public bool Equals(XDateRange dateRange)
//        {
//            return dateRange != null && this.Start == dateRange.Start && this.End == dateRange.End;
//        }

//        public override bool Equals(object obj)
//        {
//            if (!(obj is XDateRange))
//            {
//                return false;
//            }

//            return this.Equals((XDateRange)obj);

//        }

//        public override int GetHashCode()
//        {
//            int hashStart = Start.GetHashCode();
//            int hashEnd = End.GetHashCode();

//            return hashStart ^ hashEnd;
//        }

//        #endregion

//        #region To String
//        public override string ToString()
//        {
//            return string.Format("{0} To {1}", Start.ToString("dd-MM-yyyy"), End.ToString("dd-MM-yyyy"));
//        }

//        #endregion
//    }

//}

#endregion