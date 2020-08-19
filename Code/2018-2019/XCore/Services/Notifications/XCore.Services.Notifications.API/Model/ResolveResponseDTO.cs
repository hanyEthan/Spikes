namespace XCore.Services.Notifications.API.Model
{
    public class ResolveResponseDTO
    {
        public string RequestId { get; set; }
        public int MessageTemplateId { get; set; }
        public string Result { get; set; }
    }
}
