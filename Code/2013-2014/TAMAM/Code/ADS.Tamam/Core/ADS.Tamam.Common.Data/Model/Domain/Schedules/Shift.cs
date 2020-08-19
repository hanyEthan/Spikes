using System;
using System.Threading;
using System.Collections.Generic;

using ADS.Common.Models;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Model.Enums;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Schedules
{
    [Serializable]
    public class Shift : IXSerializable , XIntervals.ITimeRange , IBaseModel
    {
        #region props.

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }
        public string NameCultureVarientAbstract { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal Duration { get; set; }

        public bool IsActive { get; set; }
        public bool IsNightShift { get; set; }
        public bool IsFlexible { get; set; }

        public Guid ShiftPolicyId { get; set; }
        public PolicyModel ShiftPolicy { get; set; }

        public int? MinimumParties { get; set; }

        [XDontSerialize] public ShiftType ShiftType
        {
            get
            {
                return IsNightShift ? ShiftType.Night :
                       IsFlexible ? ShiftType.Flexible :
                       ShiftType.Normal;
            }
        }
        [XDontSerialize] public IList<ScheduleTemplateDayShifts> TemplateDetails { get; set; }

        // UI Helpers ...
        [XDontSerialize] public string StartTimeStr
        {
            get
            {
                return StartTime.HasValue ? StartTime.Value.ToString( "yyyy-MM-ddTHH:mm:ss" ) : "";
            }
        }
        [XDontSerialize] public string EndTimeStr
        {
            get
            {
                return EndTime.HasValue ? EndTime.Value.ToString( "yyyy-MM-ddTHH:mm:ss" ) : "";
            }
        }
        [XDontSerialize] public string ShiftDescription
        {
            get
            {
                if ( StartTime.HasValue && EndTime.HasValue )
                {
                    return string.Format( "{0} ({1}-{2})" ,
                        Thread.CurrentThread.CurrentCulture.Name == "ar-EG" ? NameCultureVarient : Name ,
                        StartTime.Value.ToString( "hh:mm tt" , System.Globalization.CultureInfo.InvariantCulture ) ,
                        EndTime.Value.ToString( "hh:mm tt" , System.Globalization.CultureInfo.InvariantCulture ) );
                }
                else
                {
                    return Thread.CurrentThread.CurrentCulture.Name == "ar-EG" ? NameCultureVarient : Name;
                }
            }
        }
        [XDontSerialize] public string DurationHours
        {
            get
            {
                return TimeSpan.FromHours( ( double ) Duration ).ToString( @"hh\:mm" );
            }
        }
        
        #endregion
        #region cst.

        public Shift()
        {
            this.TemplateDetails = new List<ScheduleTemplateDayShifts>();
        }

        #endregion

        #region publics.

        public static bool CheckOverlap( List<Shift> ranges )
        {
            for ( int i = 0 ; i < ranges.Count ; i++ )
            {
                for ( int j = i + 1 ; j < ranges.Count ; j++ )
                {
                    if ( ranges[i].CheckOverlap( ranges[j] ) ) return false;
                }
            }
            return true;
        }
        public bool CheckOverlap( Shift shift )
        {
            /*( start1 <= end2 and start2 <= end1 )*/
            var s = this.StartTime;
            var e = this.EndTime;
            var s1 = shift.StartTime;
            var e1 = shift.EndTime;

            if ( this.IsNightShift )
                e = e.Value.AddDays( 1 );
            if ( shift.IsNightShift )
                e1 = e1.Value.AddDays( 1 );

            return s < e1 && s1 < e;
        }
        public int GetShiftDuration()
        {
            if ( StartTime.HasValue == false || EndTime.HasValue == false ) return 0;

            DateTime start = StartTime.Value;
            DateTime end = IsNightShift ? EndTime.Value.AddDays( 1 ) : EndTime.Value;

            return ( int )( end - start ).TotalMinutes;
        }
        public string GetLocalizedName()
        {
            return Thread.CurrentThread.CurrentCulture.Name == "ar-EG" ? this.NameCultureVarient : this.Name;
        }
        public string GetLocalizedShiftType()
        {
            switch ( this.ShiftType )
            {
                case ShiftType.Night: return Resources.Culture.Schedule.Night;
                case ShiftType.Flexible: return Resources.Culture.Schedule.Flexible;
                case ShiftType.Normal:
                default: return Resources.Culture.Schedule.Normal;
            }
        }

        #endregion

        #region IBaseModel

        object IBaseModel.Id { get { return this.Id; } }
        [XDontSerialize] string IBaseModel.NameCultureVariant { get { return this.NameCultureVarient; } }

        #endregion
        #region ITimeRange

        [XDontSerialize] public bool SpansMultipleDays { get { return this.IsNightShift; } }

        #endregion
    }
}

