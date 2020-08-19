using System.Collections.Generic;

namespace XCore.Services.Notifications.API.Model
{
    public class ResolveRequestDTO
    {
        public string RequestId { get; set; }
        public int MessageTemplateId { get; set; }
        public List<MessageTemplateKeyValueDTO> Values { get; set; }
    }
}
