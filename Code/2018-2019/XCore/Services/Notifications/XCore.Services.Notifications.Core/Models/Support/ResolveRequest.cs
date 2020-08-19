using System.Collections.Generic;

namespace XCore.Services.Notifications.Core.Models.Support
{
    public class ResolveRequest
    {
        public string RequestId { get; set; }
        public int MessageTemplateId { get; set; }
        public List<MessageTemplateKeyValue> Values { get; set; }
    }
}
