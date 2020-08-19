using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class OverTimePolicy : AbstractSpecialPolicy
    {
        public float OvertimeRate { get; private set; }
        public int MaxOvertime { get; private set; }
        public float WeekendOvertimeRate { get; private set; }
        public int MaxWeekendOvertime { get; private set; }
        public float LeaveOvertimeRate { get; private set; }
        public int MaxLeaveOvertime { get; private set; }
        public float HolidayOvertimeRate { get; private set; }
        public int MaxHolidayOvertime { get; private set; }
        public bool IsRelatedToWorkingHours { get; private set; }

        public OverTimePolicy( Policy policy ) : base ( policy )
        {
            OvertimeRate = GetFloat ( PolicyFields.OvertimePolicy.OvertimeRate ) ?? 0;
            MaxOvertime = GetInt ( PolicyFields.OvertimePolicy.MaxOvertime ) ?? 0;
            WeekendOvertimeRate = GetFloat ( PolicyFields.OvertimePolicy.WeekendOvertimeRate ) ?? 0;
            MaxWeekendOvertime = GetInt ( PolicyFields.OvertimePolicy.MaxWeekendOvertime ) ?? 0;
            LeaveOvertimeRate = GetFloat ( PolicyFields.OvertimePolicy.LeaveOvertimeRate ) ?? 0;
            MaxLeaveOvertime = GetInt ( PolicyFields.OvertimePolicy.MaxLeaveOvertime ) ?? 0;
            HolidayOvertimeRate = GetFloat ( PolicyFields.OvertimePolicy.HolidayOvertimeRate ) ?? 0;
            MaxHolidayOvertime = GetInt ( PolicyFields.OvertimePolicy.MaxHolidayOvertime ) ?? 0;
            IsRelatedToWorkingHours = GetBool ( PolicyFields.OvertimePolicy.OvertimeRelatedToWorkingHours ) ?? false;
        }
    }
}
