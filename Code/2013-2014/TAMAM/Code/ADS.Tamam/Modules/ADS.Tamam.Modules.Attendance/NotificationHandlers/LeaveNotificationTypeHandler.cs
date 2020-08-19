using System;
using ADS.Common.Context;
using ADS.Common.Contracts.Notification;
using ADS.Common.Models.Domain.Notification;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;

namespace ADS.Tamam.Modules.Attendance.NotificationHandlers
{
    public class LeaveNotificationTypeHandler : INotificationTypeHandler
    {
        # region properties..

        public string Name { get { return "LeaveNotificationTypeHandler"; } }
        public bool Initialized { get { return true; } }
        public string Metadata { get { return "LeaveApproval"; } }

        # endregion

        # region publics..

        public bool Handle(INotificationTypeMetadata metadata)
        {
            var model = new LeaveReview
            {
                LeaveId = metadata.TargetId,
                Comment = metadata.Comment,
                ReviewerId = metadata.ReviewerId,
                Status = metadata.Approved ? ReviewLeaveStatus.Approved : ReviewLeaveStatus.Denied
            };

            var response = TamamServiceBroker.LeavesHandler.ReviewLeave(model, SystemRequestContext.Instance);
            return response.Type == ResponseState.Success && response.Result;
        }

        public string GetAssociatedDetails( Guid targetId )
        {
            var response = TamamServiceBroker.LeavesHandler.GetLeave( targetId , SystemRequestContext.Instance );
            var success = response.Type == ResponseState.Success;
            if ( success ) return response.Result.ToString();
            return string.Empty;
        }

        # endregion
    }
}