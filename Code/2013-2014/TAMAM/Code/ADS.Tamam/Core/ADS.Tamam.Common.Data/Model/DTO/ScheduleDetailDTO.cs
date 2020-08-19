using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;

namespace ADS.Tamam.Common.Data.Model.DTO
{
    [Serializable]
    public class ScheduleDetailDTO : IXSerializable
    {
        #region props ...

        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public string NameCultureVarient { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }

        public DateTime ScheduleStart { get; set; }
        public DateTime? ScheduleEnd { get; set; }

        public Guid ShiftId { get; set; }
        public string ShiftName { get; set; }

        public string LeaveTypeName { get; set; }
        public string LeaveTypeNameCultureVarient { get; set; }
        public string LeaveStatusName { get; set; }
        public string LeaveStatusNameCultureVarient { get; set; }

        public bool IsDayOff { get; set; }
        public ScheduleDetailDTOType EventType { get; set; }
        public string Duration { get; set; }
        public bool IsFlexible { get; set; }

        [XmlIgnore]
        [XDontSerialize]
        public Color BaseColor { get; set; }
        public string BaseColorSerializable
        {
            get { return ColorTranslator.ToHtml( BaseColor ); }
            set { BaseColor = ColorTranslator.FromHtml( value ); }
        }

        #endregion
        #region cst ...

        public ScheduleDetailDTO()
        {
        }
        public ScheduleDetailDTO( DateTime day , bool isDayOff , Shift shift , Schedule schedule , Color scheduleColor , ScheduleDetailDTOType type )
        {
            this.Id = schedule.Id;
            this.Name = schedule.Name;
            this.ScheduleStart = schedule.StartDate;
            this.ScheduleEnd = schedule.EndDate;
            this.NameCultureVarient = schedule.NameCultureVarient;
            this.Start = day;
            this.End = day;
            this.IsDayOff = isDayOff;
            this.BaseColor = scheduleColor;

            if ( shift != null )
            {
                this.ShiftStart = shift.StartTime.HasValue ? shift.StartTime.Value.ToString( "hh:mm tt" , System.Globalization.CultureInfo.InvariantCulture ) : "";
                this.ShiftEnd = shift.EndTime.HasValue ? shift.EndTime.Value.ToString( "hh:mm tt" , System.Globalization.CultureInfo.InvariantCulture ) : "";
                this.ShiftId = shift.Id;
                this.ShiftName = shift.Name;
                this.Duration = shift.Duration.ToString();
                this.IsFlexible = shift.IsFlexible;
            }

            this.EventType = type;
        }

        #endregion
        #region Utilities ...

        public static List<ScheduleDetailDTO> Map( List<ScheduleDay> models , Schedule schedule , Color scheduleColor )
        {
            var DTOs = new List<ScheduleDetailDTO>();
            var newDayShifts = new List<ScheduleDetailDTO>();
            var lastDayShifts = new List<ScheduleDetailDTO>();
            var IntermediateDayShifts = new List<ScheduleDetailDTO>();

            for ( int i = 0 ; i < models.Count ; i++ )
            {
                // ...
                var SD = models[i];
                newDayShifts = Map( SD , schedule , scheduleColor );

                foreach ( var newDayShift in newDayShifts )
                {
                    var nextLastDayShift = newDayShift;

                    foreach ( var lastDayShift in lastDayShifts )
                    {
                        if ( AreConsecutiveDays( lastDayShift , newDayShift ) )
                        {
                            lastDayShift.End = newDayShift.End.AddMilliseconds( 1 );   // extend the current piece ...
                            nextLastDayShift = lastDayShift;
                            break;
                        }
                        else
                        {
                            if ( lastDayShift.Start == lastDayShift.End ) lastDayShift.End = lastDayShift.End.AddMilliseconds( 1 );   // close the current piece, to prepare for a new one ... (even though, it could be associated in a later iteration with another newDayShift)
                        }
                    }

                    // if new piece has been chosen, without being merged with a previous piece, add it to that DTO list to be bound ...
                    if ( nextLastDayShift == newDayShift )
                    {
                        DTOs.Add( newDayShift );   // add a new piece(s) ...
                    }

                    IntermediateDayShifts.Add( nextLastDayShift );
                }

                lastDayShifts = IntermediateDayShifts;
                IntermediateDayShifts = new List<ScheduleDetailDTO>();
            }

            return DTOs;
        }
        public static List<ScheduleDetailDTO> Map( List<Leave> leaves , RandomPastelColorGenerator colorGen )
        {
            var result = new List<ScheduleDetailDTO>();
            if ( leaves != null )
            {
                foreach ( var item in leaves )
                {
                    var dto = new ScheduleDetailDTO();
                    dto.Start = item.StartDate;
                    dto.End = item.EndDate.AddDays( 1 ).AddMilliseconds( -1 );
                    dto.Name = item.Person.Name;
                    dto.NameCultureVarient = item.Person.NameArabic;
                    dto.EventType = ScheduleDetailDTOType.Leave;

                    dto.LeaveTypeName = item.LeaveType.Name;
                    dto.LeaveTypeNameCultureVarient = item.LeaveType.NameCultureVariant;
                    dto.LeaveStatusName = item.LeaveStatus.Name;
                    dto.LeaveStatusNameCultureVarient = item.LeaveStatus.NameCultureVariant;

                    result.Add( dto );
                }
            }
            return result;
        }
        public static List<ScheduleDetailDTO> Map( List<Holiday> holidays , RandomPastelColorGenerator colorGen )
        {
            var result = new List<ScheduleDetailDTO>();
            foreach ( var item in holidays )
            {
                if ( item.EndDate >= item.StartDate )
                {
                    var dto = new ScheduleDetailDTO();
                    dto.Start = item.StartDate;
                    dto.End = item.EndDate.AddDays( 1 ).AddMilliseconds( -1 );
                    dto.Name = string.Format( "{0} - {1}" , item.Name , Resources.Culture.Common.Holiday );
                    dto.NameCultureVarient = string.Format( "{0} - {1}" , item.NameCultureVariant , Resources.Culture.Common.Holiday );
                    dto.EventType = ScheduleDetailDTOType.Holiday;
                    result.Add( dto );
                }
            }
            return result;
        }
        private static List<ScheduleDetailDTO> Map( ScheduleDay model , Schedule schedule , Color scheduleColor )
        {
            var DTOs = new List<ScheduleDetailDTO>();

            if ( model.DayShifts != null && model.DayShifts.Count > 0 )
            {
                foreach ( var shift in model.DayShifts )
                {
                    DTOs.Add( new ScheduleDetailDTO( model.Day , model.IsDayOff , shift.Shift , schedule , scheduleColor , ScheduleDetailDTOType.Schedule ) );
                }
            }
            else
            {
                DTOs.Add( new ScheduleDetailDTO( model.Day , model.IsDayOff , null , schedule , scheduleColor , ScheduleDetailDTOType.Schedule ) );
            }

            return DTOs;
        }
        private static bool AreConsecutiveDays( ScheduleDetailDTO first , ScheduleDetailDTO second )
        {
            // ...
            bool isConsecutive = !( ( second.Start - first.End ).Days > 1 );
            bool hasExactShift = first.ShiftId == second.ShiftId && first.ShiftId != Guid.Empty;
            bool hasNoShifts = first.ShiftId == Guid.Empty && second.ShiftId == Guid.Empty;
            bool isWorkingDays = !first.IsDayOff && !second.IsDayOff;
            bool isOffDays = first.IsDayOff && second.IsDayOff;

            // ...
            if ( isConsecutive && isWorkingDays && hasExactShift ) return true;   // both are working days ...
            if ( isConsecutive && isOffDays && ( hasNoShifts || hasExactShift ) ) return true;   // both are off days ...
            return false;
        }

        #endregion
    }

    public enum ScheduleDetailDTOType { Schedule = 1 , Leave = 2 , Holiday = 3 };
}
