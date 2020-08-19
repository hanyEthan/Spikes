using System;
using ADS.Common.Context;
using ADS.Common.Contracts.Notification;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;

namespace ADS.Tamam.Modules.Attendance.NotificationHandlers
{
    public class ExcuseNotificationTypeHandler : INotificationTypeHandler
    {
        # region properties..

        public string Name { get { return "ExcuseNotificationTypeHandler"; } }
        public bool Initialized { get { return true; } }
        public string Metadata { get { return "ExcuseApproval"; } }

        # endregion

        # region publics..

        public bool Handle( INotificationTypeMetadata metadata )
        {
            var model = new ExcuseReview
            {
                ExcuseId = metadata.TargetId,
                Comment = metadata.Comment,
                ReviewerId = metadata.ReviewerId,
                Status = metadata.Approved ? ReviewExcuseStatus.Approved : ReviewExcuseStatus.Denied
            };

            var response = TamamServiceBroker.LeavesHandler.ReviewExcuse( model , SystemRequestContext.Instance );
            return response.Type == ResponseState.Success && response.Result;
        }

        public string GetAssociatedDetails( Guid targetId )
        {
            var response = TamamServiceBroker.LeavesHandler.GetExcuse( targetId , SystemRequestContext.Instance );
            var success = response.Type == ResponseState.Success;
            if ( success ) return response.Result.ToString();
            return string.Empty;
        }

        # endregion
    }
}