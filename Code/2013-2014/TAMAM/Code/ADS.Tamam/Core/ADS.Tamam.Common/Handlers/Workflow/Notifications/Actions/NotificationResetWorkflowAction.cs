using System;
using System.Runtime.Serialization;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;

namespace ADS.Tamam.Common.Workflow.Notifications.Actions
{
    [DataContract( IsReference = true )]
    public class NotificationResetWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                return HandlePreviousMessages( this.Step.Instance.TargetId.ToString() );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #region helpers

        private bool HandlePreviousMessages( string targetId )
        {
            // Delete in Notifications in waiting mode..
            Broker.NotificationHandler.DeleteRawNotification( targetId );

            var notifications = Broker.NotificationHandler.GetNotificationsByTarget( targetId );
            foreach ( var notification in notifications )
            {
                if ( !Broker.NotificationHandler.DeleteDetailedNotification( notification.Id ) ) return false;
            }

            return true;
        }
        
        #endregion
    }
}
