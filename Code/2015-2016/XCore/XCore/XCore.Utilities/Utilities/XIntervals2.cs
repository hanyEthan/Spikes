using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Utilities.Utilities
{
    public static class XIntervals2
    {
        #region publics.

        public static IPeriod Intersect( IPeriodDate A , IPeriodDate B )
        {
            if ( !IsOverlapped( A , B ) ) return null;
            return new PeriodDate( XDate.Max( A.Start , B.Start ) , XDate.Min( A.End , B.End ) );
        }
        public static IPeriod Intersect( IPeriodTime A , IPeriodTime B )
        {
            if ( !IsOverlapped( A , B ) ) return null;
            return new PeriodTime( XDate.Max( A.Start , B.Start ) , XDate.Min( A.End , B.End ) );
        }
        public static List<PeriodDate> Intersect( List<IPeriodDate> As , IPeriodDate B )
        {
            var Is = As.Where( x => IsOverlapped( x , B ) ).ToList();
            return Is.Select( x => new PeriodDate( x.Id , XDate.Max( B.Start , x.Start ).Value , XDate.Min( B.End , x.End ) ) ).ToList();
        }
        public static List<PeriodDate> Intersect( List<IPeriodDate> As , List<IPeriodDate> Bs )
        {
            #region validate

            var emptyAs = As == null || As.Count == 0;
            var emptyBs = Bs == null || Bs.Count == 0;

            if ( emptyAs && emptyBs ) return new List<PeriodDate>();
            if ( emptyAs ) return Bs.Select( x => new PeriodDate( x ) ).ToList();
            if ( emptyBs ) return As.Select( x => new PeriodDate( x ) ).ToList();

            #endregion
            #region normalize

            // replace nulls with min / max dates ...

            foreach ( var A in As.Where( x => x.Start == null ) ) { A.Start = DateTime.MinValue.Date; }
            foreach ( var A in As.Where( x => x.End == null ) ) { A.End = DateTime.MaxValue.Date; }
            foreach ( var B in Bs.Where( x => x.Start == null ) ) { B.Start = DateTime.MinValue.Date; }
            foreach ( var B in Bs.Where( x => x.End == null ) ) { B.End = DateTime.MaxValue.Date; }

            // order ...

            As = As.OrderBy( x => x.Start ).ToList();
            Bs = Bs.OrderBy( x => x.Start ).ToList();

            #endregion
            #region prepare

            // ...
            var result = new List<PeriodDate>();

            // priority : Bs ...
            result.AddRange( Bs.Select( x => new PeriodDate( x ) ).ToList() );

            // global range ...
            var GStart = XDate.Min( As[0].Start , Bs[0].Start );
            var GEnd = XDate.Max( As[As.Count - 1].End , Bs[Bs.Count - 1].End );

            #endregion
            #region gaps

            // gaps ...
            foreach ( var B in Bs )
            {
                if ( GStart < B.Start )
                {
                    var gap = new PeriodDate( GStart.Value , B.Start.Value.AddDays( -1 ) );
                    result.AddRange( Intersect( As , gap ) );
                }

                if ( XDate.IsMax( B.End ) ) break;

                GStart = B.End.Value.AddDays( 1 );
            }

            // last gap ...
            if ( XDate.IsMax( Bs.Last().End ) )
            {
                if ( GStart <= GEnd )
                {
                    var gap = new PeriodDate( GStart , GEnd );
                    result.AddRange( Intersect( As , gap ) );
                }
            }

            #endregion
            #region finalize

            // replace max dates with nulls ...
            foreach ( var R in result.Where( x => x.End >= DateTime.MaxValue.AddDays( -1 ) ) ) { R.End = null; }

            // order ...
            result = result.OrderBy( x => x.Start ).ToList();

            // ...
            return result;

            #endregion
        }

        public static bool IsOverlapped( IPeriodDate A , IPeriodDate B )
        {
            A.Order();
            B.Order();

            return ( !A.Start.HasValue || !B.End.HasValue || A.Start.Value <= B.End.Value ) &&
                   ( !A.End.HasValue || !B.Start.HasValue || A.End.Value >= B.Start.Value );
        }
        public static bool IsOverlapped( IPeriodTime A , IPeriodTime B )
        {
            A.Order();
            B.Order();

            if ( A.IsOverNight && !B.IsOverNight )   // first is nightly / second is normal
            {
                return ( !A.End.HasValue || !B.Start.HasValue || A.End.Value >= B.Start.Value ) ||
                       ( !A.Start.HasValue || !B.End.HasValue || A.Start.Value <= B.End.Value );
            }
            else if ( B.IsOverNight && !A.IsOverNight )   // first is normal / second is nightly
            {
                return ( !B.End.HasValue || !A.Start.HasValue || B.End.Value >= A.Start.Value ) ||
                       ( !B.Start.HasValue || !A.End.HasValue || B.Start.Value <= A.End.Value );
            }
            else if ( !A.IsOverNight && !B.IsOverNight )   // first is normal / second is normal
            {
                return ( !A.Start.HasValue || !B.End.HasValue || A.Start.Value <= B.End.Value ) &&
                       ( !A.End.HasValue || !B.Start.HasValue || A.End.Value >= B.Start.Value );
            }
            else   // first is nightly / second is nightly
            {
                return ( !A.Start.HasValue || !B.End.HasValue || A.Start.Value <= B.End.Value ) ||
                       ( !A.End.HasValue || !B.Start.HasValue || A.End.Value >= B.Start.Value );
            }
        }

        public static bool IsPoint( IPeriodDate A )
        {
            return A.Start.HasValue && A.End.HasValue && A.Start.Value == A.End.Value;
        }
        public static bool IsPoint( IPeriodTime A )
        {
            return A.Start.HasValue && A.End.HasValue && A.Start.Value == A.End.Value;
        }

        public static bool IsSubset( IPeriodDate A , IPeriodDate B )
        {
            return IsSubset( A , B , true );
        }
        public static bool IsSubset( IPeriodDate A , IPeriodDate B , bool inclusive )
        {
            return ( B.Start.HasValue || A.Start >= B.Start ) &&
                   ( B.End.HasValue || A.End <= B.End ) &&
                   ( inclusive || A.Start != B.Start || A.End != B.End );
        }
        public static bool IsSubset( IPeriodTime A , IPeriodTime B )
        {
            return IsSubset( A , B , true );
        }
        public static bool IsSubset( IPeriodTime A , IPeriodTime B , bool inclusive )
        {
            A.Order();
            B.Order();

            return ( B.Start.HasValue || A.Start >= B.Start ) &&
                   ( B.End.HasValue || A.End <= B.End ) &&
                   ( inclusive || A.Start != B.Start || A.End != B.End );
        }

        public static IPeriodDate Union( IPeriodDate A , IPeriodDate B )
        {
            return IsOverlapped( A , B ) ? new PeriodDate( XDate.Min( A.Start , B.Start ) , XDate.Max( A.End , B.End ) )
                                         : null;
        }
        public static IPeriodTime Union( IPeriodTime A , IPeriodTime B )
        {
            return IsOverlapped( A , B ) ? new PeriodTime( XDate.Min( A.Start , B.Start ) , XDate.Max( A.End , B.End ) )
                                         : null;
        }

        public static IPeriodTime Nearest( DateTime time , List<IPeriodTime> list , bool isOut )
        {
            return Nearest( time , list , new TimeSpan() , new TimeSpan() , isOut );
        }
        public static IPeriodTime Nearest( DateTime time , List<IPeriodTime> list , TimeSpan marginBefore , TimeSpan marginAfter , bool isOut )
        {
            // ...
            if ( list == null || list.Count == 0 ) return null;

            // ...
            var unlimitedTimeRange = list.Where( x => !x.Start.HasValue || !x.End.HasValue ).FirstOrDefault();
            if ( unlimitedTimeRange != null ) return unlimitedTimeRange;

            // ...
            var point = NormalizeTime( time );

            // ...
            var intersected = list.Where( x => !x.IsOverNight ? ( point >= NormalizeTime( x.Start ) && point <= NormalizeTime( x.End ) )
                                                              : ( point >= NormalizeTime( x.Start ) || point <= NormalizeTime( x.End ) ) ).FirstOrDefault();
            if ( intersected != null ) return intersected;

            // ...
            if ( !isOut )
            {
                return list.Where( x => !x.IsOverNight ? ( point >= UpdateTimeWithMarginBefore( NormalizeTime( x.Start ) , marginBefore ) && point <= NormalizeTime( x.End ) )
                                                       : ( point >= UpdateTimeWithMarginBefore( NormalizeTime( x.Start ) , marginBefore ) || point <= NormalizeTime( x.End ) ) )

                           .OrderBy( x => GetDuration( x.Start.Value , point ) ).FirstOrDefault();
            }
            else
            {
                return list.Where( x => !x.IsOverNight ? ( point >= NormalizeTime( x.Start ) && point <= UpdateTimeWithMarginAfter( NormalizeTime( x.End ) , isOut ? ( TimeSpan? ) marginAfter : null ) )
                                                       : ( point >= NormalizeTime( x.Start ) || point <= UpdateTimeWithMarginAfter( NormalizeTime( x.End ) , isOut ? ( TimeSpan? ) marginAfter : null ) ) )

                           .OrderBy( x => GetDuration( x.Start.Value , point ) ).FirstOrDefault();
            }
        }
        public static IPeriodTime Nearest( DateTime time , List<IPeriodTime> list , Dictionary<Guid , PeriodTime> margins , bool isOut )
        {
            // ...
            if ( list == null || list.Count == 0 ) return null;

            // ...
            var unlimitedTimeRange = list.Where( x => !x.Start.HasValue || !x.End.HasValue ).FirstOrDefault();
            if ( unlimitedTimeRange != null ) return unlimitedTimeRange;

            // ...
            var point = NormalizeTime( time );

            // ...
            var intersected = list.Where( x => !x.IsOverNight ? ( point >= NormalizeTime( x.Start ) && point <= NormalizeTime( x.End ) )
                                                              : ( point >= NormalizeTime( x.Start ) || point <= NormalizeTime( x.End ) ) ).FirstOrDefault();
            if ( intersected != null ) return intersected;

            // ...
            if ( !isOut )
            {
                return list.Where( x => !x.IsOverNight ? ( point >= UpdateTimeWithMarginBefore( NormalizeTime( x.Start ) , margins[x.Id].Start.Value ) && point <= NormalizeTime( x.End ) )
                                                       : ( point >= UpdateTimeWithMarginBefore( NormalizeTime( x.Start ) , margins[x.Id].Start.Value ) || point <= NormalizeTime( x.End ) ) )

                           .OrderBy( x => GetDuration( x.Start.Value , point ) ).FirstOrDefault();
            }
            else
            {
                return list.Where( x => !x.IsOverNight ? ( point >= NormalizeTime( x.Start ) && point <= UpdateTimeWithMarginAfter( NormalizeTime( x.End ) , isOut ? ( TimeSpan? ) margins[x.Id].End : null ) )
                                                       : ( point >= NormalizeTime( x.Start ) || point <= UpdateTimeWithMarginAfter( NormalizeTime( x.End ) , isOut ? ( TimeSpan? ) margins[x.Id].End : null ) ) )

                           .OrderBy( x => GetDuration( x.Start.Value , point ) ).FirstOrDefault();
            }
        }
        public static IPeriodTime Nearest( DateTime time , List<IPeriodTime> list , Dictionary<Guid , PeriodTime> margins )
        {
            // ...
            if ( list == null || list.Count == 0 ) return null;

            // ...
            var unlimitedTimeRange = list.Where( x => !x.Start.HasValue || !x.End.HasValue ).FirstOrDefault();
            if ( unlimitedTimeRange != null ) return unlimitedTimeRange;

            // ...
            var point = NormalizeTime( time );

            // ...
            var intersected = list.Where( x => !x.IsOverNight ? ( point >= NormalizeTime( x.Start ) && point <= NormalizeTime( x.End ) )
                                                              : ( point >= NormalizeTime( x.Start ) || point <= NormalizeTime( x.End ) ) ).FirstOrDefault();
            if ( intersected != null ) return intersected;

            // ...      

            return list.Where( x => !x.IsOverNight ? ( point >= UpdateTimeWithMarginBefore( NormalizeTime( x.Start ) , margins[x.Id].Start.Value ) && point <= UpdateTimeWithMarginAfter( NormalizeTime( x.End ) , ( TimeSpan? ) margins[x.Id].End ) )
                                                   : ( point >= UpdateTimeWithMarginBefore( NormalizeTime( x.Start ) , margins[x.Id].Start.Value ) || point <= UpdateTimeWithMarginAfter( NormalizeTime( x.End ) , ( TimeSpan? ) margins[x.Id].End ) ) )
                       .OrderBy( x => GetMinDuration( x.Start.Value , x.End.Value , point.TimeOfDay ) ).FirstOrDefault();
        }

        #endregion
        #region helpers.

        private static DateTime NormalizeTime( DateTime dateTime )
        {
            return new DateTime().AddTicks( dateTime.TimeOfDay.Ticks );
        }
        private static DateTime NormalizeTime( DateTime? dateTime )
        {
            return new DateTime().AddTicks( dateTime.Value.TimeOfDay.Ticks );
        }
        private static DateTime NormalizeTime( TimeSpan time )
        {
            return new DateTime().AddTicks( time.Ticks );
        }
        private static DateTime NormalizeTime( TimeSpan? time )
        {
            return new DateTime().AddTicks( time.Value.Ticks );
        }

        private static DateTime UpdateTimeWithMarginBefore( DateTime time , TimeSpan? margin )
        {
            if ( !margin.HasValue ) return time;

            var marginEffective = new TimeSpan( 0 , XMath.Round( -margin.Value.TotalMinutes ) , 0 );
            var marginOffset = marginEffective.TotalMinutes;
            var DayOffset = ( new TimeSpan() - time.TimeOfDay ).TotalMinutes;

            return time.AddMinutes( ( time.TimeOfDay.Add( marginEffective ) ).TotalMinutes < 0 ? DayOffset : marginOffset );
        }
        private static DateTime UpdateTimeWithMarginAfter( DateTime time , TimeSpan? margin )
        {
            var marginEffective = margin.HasValue ? new TimeSpan( 0 , XMath.Round( margin.Value.TotalMinutes ) , 0 ) : new TimeSpan();

            return time.AddMinutes( XMath.Round( marginEffective.TotalMinutes ) );
        }

        private static int GetDuration( DateTime from , DateTime to )
        {
            return XMath.Round( Math.Abs( ( from.TimeOfDay - to.TimeOfDay ).TotalMinutes ) );
        }
        private static int GetDuration( TimeSpan from , TimeSpan to )
        {
            return XMath.Round( Math.Abs( ( from - to ).TotalMinutes ) );
        }
        private static int GetDuration( TimeSpan from , DateTime to )
        {
            return XMath.Round( Math.Abs( ( from - to.TimeOfDay ).TotalMinutes ) );
        }

        private static int GetMinDuration( TimeSpan start , TimeSpan end , TimeSpan to )
        {
            var Duration1 = XMath.Round( Math.Abs( ( start - to ).TotalMinutes ) );
            var Duration2 = XMath.Round( Math.Abs( ( end - to ).TotalMinutes ) );
            return Math.Min( Duration1 , Duration2 );
        }

        #endregion
        #region nested.

        public interface IPeriod
        {
            Guid Id { get; set; }
            string Name { get; set; }

            void Order();
        }
        public interface IPeriodDate : IPeriod
        {
            DateTime? Start { get; set; }
            DateTime? End { get; set; }
        }
        public interface IPeriodTime : IPeriod
        {
            TimeSpan? Start { get; set; }
            TimeSpan? End { get; set; }

            bool IsOverNight { get; set; }
        }
        public class PeriodDate : IPeriodDate
        {
            #region props.

            public Guid Id { get; set; }
            public string Name { get; set; }

            public DateTime? Start { get; set; }
            public DateTime? End { get; set; }

            public IPeriodDate Reference { get; set; }

            #endregion
            #region cst.

            public PeriodDate()
            {

            }
            public PeriodDate( IPeriodDate reference )
            {
                if ( reference == null ) return;

                this.Id = reference.Id;
                this.Name = reference.Name;
                this.Start = reference.Start;
                this.End = reference.End;
                this.Reference = reference;
            }
            public PeriodDate( DateTime? start , DateTime? end )
            {
                this.Start = start;
                this.End = end;
            }
            public PeriodDate( Guid id , DateTime? start , DateTime? end ) : this( start , end )
            {
                this.Id = id;
            }

            #endregion

            #region publics.

            public void Order()
            {
                if ( !this.Start.HasValue || !this.End.HasValue ) return;
                if ( this.Start.HasValue && this.End.HasValue && this.End.Value > this.Start.Value ) return;

                SwapMargins();
            }

            #endregion
            #region helpers

            private void SwapMargins()
            {
                var x = this.Start;
                this.Start = this.End;
                this.End = x;
            }

            #endregion
        }
        public class PeriodTime : IPeriodTime
        {
            #region props.

            public Guid Id { get; set; }
            public string Name { get; set; }

            public TimeSpan? Start { get; set; }
            public TimeSpan? End { get; set; }

            public bool IsOverNight { get; set; }

            #endregion
            #region cst.

            public PeriodTime()
            {

            }
            public PeriodTime( TimeSpan? start , TimeSpan? end )
            {
                this.Start = start;
                this.End = end;
            }

            #endregion

            #region publics.

            public void Order()
            {
                if ( this.IsOverNight ) return;
                if ( !this.Start.HasValue || !this.End.HasValue ) return;
                if ( this.Start.HasValue && this.End.HasValue && this.End.Value > this.Start.Value ) return;

                SwapMargins();
            }

            #endregion
            #region helpers

            private void SwapMargins()
            {
                var x = this.Start;
                this.Start = this.End;
                this.End = x;
            }

            #endregion
        }

        #endregion
    }
}
