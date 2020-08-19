using System;
using ADS.Common.Context;
using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Notification;
using ADS.Tamam.Common.Data.Model.Domain.Attendance;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;

namespace ADS.Tamam.Modules.Attendance.NotificationHandlers
{
    public class AttendanceNotificationTypeHandler : INotificationTypeHandler
    {
        # region properties..

        public string Name { get { return "AttendanceNotificationTypeHandler"; } }
        public bool Initialized { get { return true; } }
        public string Metadata { get { return "LateAttendance"; } }

        # endregion

        # region publics..

        public bool Handle(INotificationTypeMetadata metadata)
        {
            var model = new AttendanceViolationReview
            {
                ScheduleEventId = metadata.TargetId,
                Comment = metadata.Comment,
                ReviewerId = metadata.ReviewerId,
                Status = metadata.Approved ? ReviewAttendanceStatus.Approved : ReviewAttendanceStatus.Denied
            };

            var response = TamamServiceBroker.AttendanceHandler.ReviewAttendanceViolation( model , SystemRequestContext.Instance );
            return response.Type == ResponseState.Success && response.Result;
        }

        public string GetAssociatedDetails( Guid targetId )
        {
            var response = SystemBroker.AttendanceHandler.GetScheduleEvent( targetId );
            var success = response.Type == ResponseState.Success;
            if ( success ) return response.Result.ToString();
            return string.Empty;
        }

        # endregion
    }
}