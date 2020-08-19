﻿using System;
using ADS.Common.Models.Enums;

namespace ADS.Common.Models.Domain.Notification
{
    [Serializable]
    public class AttendanceManualEditNotification : NotificationMessage
    {
        public override NotificationDetailedMessage GetDetailedMessage()
        {
            NotificationDetailedMessage message = new AttendanceManualEditNotificationDetailedMessage();

            message.Code = Code;
            message.Message = Message;
            message.MessageCultureVariant = MessageCultureVariant;
            message.Type = Type;
            message.ActionName = ActionName;
            message.ActionNameCultureVariant = ActionNameCultureVariant;
            message.ActionUrl = ActionUrl;
            message.PersonId = PersonId;
            message.TargetId = TargetId;
            message.CreationTime = CreationTime;
            message.Status = NotificationStatus.Pending;
            message.IsRead = false;
            message.Metadata = "AttendanceManualEdit";

            return message;
        }
    }
}