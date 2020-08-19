using System;
using ADS.Common.Models.Domain.Notification;

namespace ADS.Common.Contracts.Notification
{
    public interface INotificationPolicy
    {
        bool EnableNotifications { get; }

        bool InLate_Tamam { get; }
        bool InLate_Email { get; }
        bool InLate_SMS { get; }
        TimeSpan InLate_Delay { get; }

        bool LateAbsent_Tamam { get; }
        bool LateAbsent_Email { get; }
        bool LateAbsent_SMS { get; }
        TimeSpan LateAbsent_Delay { get; }

        bool Absent_Tamam { get; }
        bool Absent_Email { get; }
        bool Absent_SMS { get; }
        TimeSpan Absent_Delay { get; }

        bool EarlyLeave_Tamam { get; }
        bool EarlyLeave_Email { get; }
        bool EarlyLeave_SMS { get; }
        TimeSpan EarlyLeave_Delay { get; }

        bool LeaveOnGrace_Tamam { get; }
        bool LeaveOnGrace_Email { get; }
        bool LeaveOnGrace_SMS { get; }
        TimeSpan LeaveOnGrace_Delay { get; }

        bool MissedPunches_Tamam { get; }
        bool MissedPunches_Email { get; }
        bool MissedPunches_SMS { get; }
        TimeSpan MissedPunches_Delay { get; }

        bool Leaves_Tamam { get; }
        bool Leaves_Email { get; }
        bool Leaves_SMS { get; }
        TimeSpan Leaves_Delay { get; }

        bool Excuses_Tamam { get; }
        bool Excuses_Email { get; }
        bool Excuses_SMS { get; }
        TimeSpan Excuses_Delay { get; }

        bool AttendanceManualEdit_Tamam { get; }
        bool AttendanceManualEdit_Email { get; }
        bool AttendanceManualEdit_SMS { get; }
        TimeSpan AttendanceManualEdit_Delay { get; }

        void ProcessNotification( NotificationMessage message );
    }
}