using System;
using ADS.Common.Context;
using ADS.Common.Contracts.Notification;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Attendance;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;

namespace ADS.Tamam.Modules.Attendance.NotificationHandlers
{
    public class AttendanceManualEditNotificationTypeHandler : INotificationTypeHandler
    {
        # region properties..

        public string Name { get { return "AttendanceManualEditNotificationTypeHandler"; } }
        public bool Initialized { get { return true; } }
        public string Metadata { get { return "AttendanceManualEdit"; } }

        # endregion

        # region publics..

        public bool Handle( INotificationTypeMetadata metadata )
        {
            var model = new AttendanceManualEditReview
            {
                AttendanceRawDataHistoryId = metadata.TargetId,
                Comment = metadata.Comment,
                ReviewerId = metadata.ReviewerId,
                Status = metadata.Approved ? ReviewAttendanceStatus.Approved : ReviewAttendanceStatus.Denied
            };

            var response = TamamServiceBroker.AttendanceHandler.ReviewAttendanceManualEdit( model , SystemRequestContext.Instance );
            return response.Type == ResponseState.Success && response.Result;
        }

        public string GetAssociatedDetails( Guid targetId )
        {
            var response = TamamServiceBroker.AttendanceHandler.GetAttendanceRawDataHistoryItem( targetId , SystemRequestContext.Instance );
            var success = response.Type == ResponseState.Success;
            if ( success ) return response.Result.ToString();
            return string.Empty;
        }

        # endregion
    }
}