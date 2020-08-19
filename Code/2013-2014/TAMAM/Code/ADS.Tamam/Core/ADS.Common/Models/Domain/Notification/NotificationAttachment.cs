using System;

namespace ADS.Common.Models.Domain.Notification
{
    [Serializable]
    public class NotificationAttachment
    {
        public string Name { get; set; }
        public string MIMEType { get; set; }
        public byte[] Report { get; set; }
    }
}