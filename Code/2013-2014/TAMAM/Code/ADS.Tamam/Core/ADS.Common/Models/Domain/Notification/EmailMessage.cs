using System;
using System.Collections.Generic;

namespace ADS.Common.Models.Domain.Notification
{
    public class EmailMessage
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string To { get; set; }
        public string CCs { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentsSerialized { get; set; }

        public List<NotificationAttachment> Attachments { get; set; }

        #region cst ...

        public EmailMessage() { }
        public EmailMessage( string code, string to, string ccs, string subject, string body, string attachments )
        {
            this.Id = Guid.NewGuid();

            this.Code = code;
            this.To = to;
            this.CCs = ccs;
            this.Subject = subject;
            this.Body = body;
            this.AttachmentsSerialized = attachments;
        }

        #endregion
    }
}