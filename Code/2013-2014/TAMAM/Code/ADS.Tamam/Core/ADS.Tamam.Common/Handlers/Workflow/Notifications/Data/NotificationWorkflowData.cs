using System.Runtime.Serialization;
using ADS.Common.Models.Enums;
using ADS.Common.Workflow.Models;

namespace ADS.Tamam.Common.Workflow.Notifications.Data
{
    [DataContract( IsReference = true )]
    public class NotificationWorkflowData : WorkflowBaseData
    {
        [DataMember] public string Code { get; set; }
        [DataMember] public string TargetId { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public string MessageCultureVariant { get; set; }
        [DataMember] public string DelegateMessage { get; set; }
        [DataMember] public string DelegateMessageCultureVariant { get; set; }
        [DataMember] public NotificationType Type { get; set; }
        [DataMember] public string ActionUrl { get; set; }
        [DataMember] public string TargetType { get; set; }
        [DataMember] public NotificationTargetType NotificationTargetType { get; set; }
        [DataMember] public bool IncludeDelegate { get; set; }
        [DataMember] public string CCs { get; set; }

        #region cst ...

        public NotificationWorkflowData()
        {
        }
        public NotificationWorkflowData( string code , string personId , string targetId, string message , string messageCultureVariant , string delegateMessage , string delegateMessageCultureVariant , NotificationType type , string actionUrl , string targetType , NotificationTargetType notificationTargetType , bool includeDelegate , string ccs ) : this()
        {
            Code = code;
            PersonId = personId;
            TargetId = targetId;
            Message = message;
            MessageCultureVariant = messageCultureVariant;
            DelegateMessage = delegateMessage;
            DelegateMessageCultureVariant = delegateMessageCultureVariant;
            Type = type;
            ActionUrl = actionUrl;
            TargetType = targetType;
            NotificationTargetType = notificationTargetType;
            IncludeDelegate = includeDelegate;
            CCs = ccs;
        }

        #endregion
    }
}
