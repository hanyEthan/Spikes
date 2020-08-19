using System;
using System.Linq;
using ADS.Common.Models.Enums;

namespace ADS.Common.Models.Domain.Notification
{
    [Serializable]
    public class ScheduledReportNotificationMessage : NotificationMessage
    {
        public override NotificationDetailedMessage GetDetailedMessage()
        {
            NotificationDetailedMessage message = new ScheduledReportNotificationDetailedMessage();

            string date = DateTime.Today.ToString( "dd/MM/yyyy" );
            string reports = "";

            if (Attachments != null) reports = string.Join( ", " , Attachments.Select( x => x.Name ).ToList() );
            
            string messageString_EN = string.Format( Messages.EN , reports , date );
            string messageString_AR = string.Format( Messages.AR , reports , date );

            message.Code = Code;
            message.Message = messageString_EN;
            message.MessageCultureVariant = messageString_AR;

            message.Type = Type;
            message.ActionName = string.Empty;
            message.ActionNameCultureVariant = string.Empty;
            message.ActionUrl = string.Empty;
            message.PersonId = PersonId;
            message.TargetId = string.Empty;
            message.CreationTime = CreationTime;
            message.Status = NotificationStatus.Pending;
            message.IsRead = false;
            message.Metadata = "ScheduledReport";

            return message;
        }

        private class Messages
        {
            public const string EN = "Your scheduled reports ({0}) sent to your email at {1}";
            public const string AR = "تم ارسال تقاريرك المجدولة ({0}) الى بريدك الالكترونى بتاريخ {1}";
        }
    }
}