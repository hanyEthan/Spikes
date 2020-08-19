namespace XCore.Framework.Infrastructure.Messaging.Queues.Models.Enums
{
    public enum QueueMessageType
    {
        Notification = 0,
        NotificationEmail = 1,
        SimilarProduct = 2,

        ResetPassword = 10,
        BanProduct = 40,
        Thumbnails = 50,
        EntityNameChanged = 60,
        Logging = 70,
        CompanyDataChanged = 80,
        UserProfileChanged = 90,
    }
}
