using System;
using System.Collections.Generic;
using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Notification;
using ADS.Common.Models.Enums;
using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class NotificationsPolicy : AbstractSpecialPolicy , INotificationPolicy
    {
        # region Fields..

        private readonly TimeSpan DefaultDelayTime = new TimeSpan( 0 , 0 , 0 );

        # endregion
        #region Properties

        public bool EnableNotifications { get; private set; }

        public bool InLate_Tamam { get; private set; }
        public bool InLate_Email { get; private set; }
        public bool InLate_SMS { get; private set; }
        public TimeSpan InLate_Delay { get; private set; }

        public bool LateAbsent_Tamam { get; private set; }
        public bool LateAbsent_Email { get; private set; }
        public bool LateAbsent_SMS { get; private set; }
        public TimeSpan LateAbsent_Delay { get; private set; }

        public bool Absent_Tamam { get; private set; }
        public bool Absent_Email { get; private set; }
        public bool Absent_SMS { get; private set; }
        public TimeSpan Absent_Delay { get; private set; }

        public bool EarlyLeave_Tamam { get; private set; }
        public bool EarlyLeave_Email { get; private set; }
        public bool EarlyLeave_SMS { get; private set; }
        public TimeSpan EarlyLeave_Delay { get; private set; }

        public bool LeaveOnGrace_Tamam { get; private set; }
        public bool LeaveOnGrace_Email { get; private set; }
        public bool LeaveOnGrace_SMS { get; private set; }
        public TimeSpan LeaveOnGrace_Delay { get; private set; }

        public bool MissedPunches_Tamam { get; private set; }
        public bool MissedPunches_Email { get; private set; }
        public bool MissedPunches_SMS { get; private set; }
        public TimeSpan MissedPunches_Delay { get; private set; }

        public bool Leaves_Tamam { get; private set; }
        public bool Leaves_Email { get; private set; }
        public bool Leaves_SMS { get; private set; }
        public TimeSpan Leaves_Delay { get; private set; }

        public bool Excuses_Tamam { get; private set; }
        public bool Excuses_Email { get; private set; }
        public bool Excuses_SMS { get; private set; }
        public TimeSpan Excuses_Delay { get; private set; }

        public bool AttendanceManualEdit_Tamam { get; private set; }
        public bool AttendanceManualEdit_Email { get; private set; }
        public bool AttendanceManualEdit_SMS { get; private set; }
        public TimeSpan AttendanceManualEdit_Delay { get; private set; }

        #endregion
        #region Cst..

        public NotificationsPolicy( Policy policy ) : base( policy )
        {
            EnableNotifications = GetBool( PolicyFields.NotificationsPolicy.EnableNotifications ) ?? false;

            InLate_Tamam = GetBool( PolicyFields.NotificationsPolicy.InLate_Tamam ) ?? false;
            InLate_Email = GetBool( PolicyFields.NotificationsPolicy.InLate_Email ) ?? false;
            InLate_SMS = GetBool( PolicyFields.NotificationsPolicy.InLate_SMS ) ?? false;
            InLate_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.InLate_Delay ) ?? DefaultDelayTime;

            LateAbsent_Tamam = GetBool( PolicyFields.NotificationsPolicy.LateAbsent_Tamam ) ?? false;
            LateAbsent_Email = GetBool( PolicyFields.NotificationsPolicy.LateAbsent_Email ) ?? false;
            LateAbsent_SMS = GetBool( PolicyFields.NotificationsPolicy.LateAbsent_SMS ) ?? false;
            LateAbsent_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.LateAbsent_Delay ) ?? DefaultDelayTime;

            Absent_Tamam = GetBool( PolicyFields.NotificationsPolicy.Absent_Tamam ) ?? false;
            Absent_Email = GetBool( PolicyFields.NotificationsPolicy.Absent_Email ) ?? false;
            Absent_SMS = GetBool( PolicyFields.NotificationsPolicy.Absent_SMS ) ?? false;
            Absent_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.Absent_Delay ) ?? DefaultDelayTime;

            EarlyLeave_Tamam = GetBool( PolicyFields.NotificationsPolicy.EarlyLeave_Tamam ) ?? false;
            EarlyLeave_Email = GetBool( PolicyFields.NotificationsPolicy.EarlyLeave_Email ) ?? false;
            EarlyLeave_SMS = GetBool( PolicyFields.NotificationsPolicy.EarlyLeave_SMS ) ?? false;
            EarlyLeave_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.EarlyLeave_Delay ) ?? DefaultDelayTime;

            LeaveOnGrace_Tamam = GetBool(PolicyFields.NotificationsPolicy.LeaveOnGrace_Tamam) ?? false;
            LeaveOnGrace_Email = GetBool(PolicyFields.NotificationsPolicy.LeaveOnGrace_Email) ?? false;
            LeaveOnGrace_SMS = GetBool(PolicyFields.NotificationsPolicy.LeaveOnGrace_SMS) ?? false;
            LeaveOnGrace_Delay = GetTimeSpan(PolicyFields.NotificationsPolicy.LeaveOnGrace_Delay) ?? DefaultDelayTime;

            MissedPunches_Tamam = GetBool( PolicyFields.NotificationsPolicy.MissedPunches_Tamam ) ?? false;
            MissedPunches_Email = GetBool( PolicyFields.NotificationsPolicy.MissedPunches_Email ) ?? false;
            MissedPunches_SMS = GetBool( PolicyFields.NotificationsPolicy.MissedPunches_SMS ) ?? false;
            MissedPunches_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.MissedPunches_Delay ) ?? DefaultDelayTime;

            Leaves_Tamam = GetBool( PolicyFields.NotificationsPolicy.Leaves_Tamam ) ?? false;
            Leaves_Email = GetBool( PolicyFields.NotificationsPolicy.Leaves_Email ) ?? false;
            Leaves_SMS = GetBool( PolicyFields.NotificationsPolicy.Leaves_SMS ) ?? false;
            Leaves_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.Leaves_Delay ) ?? DefaultDelayTime;

            Excuses_Tamam = GetBool( PolicyFields.NotificationsPolicy.Excuses_Tamam ) ?? false;
            Excuses_Email = GetBool( PolicyFields.NotificationsPolicy.Excuses_Email ) ?? false;
            Excuses_SMS = GetBool( PolicyFields.NotificationsPolicy.Excuses_SMS ) ?? false;
            Excuses_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.Excuses_Delay ) ?? DefaultDelayTime;

            AttendanceManualEdit_Tamam = GetBool( PolicyFields.NotificationsPolicy.AttendanceManualEdit_Tamam ) ?? false; ;
            AttendanceManualEdit_Email = GetBool( PolicyFields.NotificationsPolicy.AttendanceManualEdit_Email ) ?? false; ;
            AttendanceManualEdit_SMS = GetBool( PolicyFields.NotificationsPolicy.AttendanceManualEdit_SMS ) ?? false; ;
            AttendanceManualEdit_Delay = GetTimeSpan( PolicyFields.NotificationsPolicy.AttendanceManualEdit_Delay ) ?? DefaultDelayTime;
        }

        #endregion
        #region Methods

        public void ProcessNotification( NotificationMessage message )
        {
            List<string> tokens;

            switch ( message.TargetType )
            {
                case NotificationTargetType.InLate:

                    tokens = new List<string>();
                    if ( InLate_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && InLate_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && InLate_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = InLate_Delay.ToString();

                    break;
                case NotificationTargetType.LateAbsent:

                    tokens = new List<string>();
                    if ( LateAbsent_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && LateAbsent_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && LateAbsent_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = LateAbsent_Delay.ToString();

                    break;
                case NotificationTargetType.Absent:

                    tokens = new List<string>();
                    if ( Absent_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && Absent_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && Absent_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = Absent_Delay.ToString();

                    break;
                case NotificationTargetType.EarlyLeave:

                    tokens = new List<string>();
                    if ( EarlyLeave_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && EarlyLeave_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && EarlyLeave_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = EarlyLeave_Delay.ToString();

                    break;

                case NotificationTargetType.WorkingLess:

                    tokens = new List<string>();
                    if (LeaveOnGrace_Tamam) tokens.Add(NotificationSubscribersTokens.TamamSubscriber);
                    if (EnableNotifications && LeaveOnGrace_Email) tokens.Add(NotificationSubscribersTokens.EmailSubscriber);
                    if (EnableNotifications && LeaveOnGrace_SMS) tokens.Add(NotificationSubscribersTokens.SMSSubscriber);

                    message.SubscribersTokens = string.Join(",", tokens);
                    message.DelayTime = LeaveOnGrace_Delay.ToString();

                    break;

                case NotificationTargetType.MissedPunch:

                    tokens = new List<string>();
                    if ( MissedPunches_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && MissedPunches_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && MissedPunches_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = MissedPunches_Delay.ToString();

                    break;
                case NotificationTargetType.AttendanceManualEdit:

                    tokens = new List<string>();
                    if ( AttendanceManualEdit_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && AttendanceManualEdit_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && AttendanceManualEdit_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = AttendanceManualEdit_Delay.ToString();

                    break;

                case NotificationTargetType.Leaves:

                    tokens = new List<string>();
                    if ( Leaves_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && Leaves_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && Leaves_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = Leaves_Delay.ToString();

                    break;
                case NotificationTargetType.Excuses:

                    tokens = new List<string>();
                    if ( Excuses_Tamam ) tokens.Add( NotificationSubscribersTokens.TamamSubscriber );
                    if ( EnableNotifications && Excuses_Email ) tokens.Add( NotificationSubscribersTokens.EmailSubscriber );
                    if ( EnableNotifications && Excuses_SMS ) tokens.Add( NotificationSubscribersTokens.SMSSubscriber );

                    message.SubscribersTokens = string.Join( "," , tokens );
                    message.DelayTime = Excuses_Delay.ToString();

                    break;
            }
        }

        #endregion
    }
}