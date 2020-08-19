namespace XCore.Utilities.Infrastructure.Messaging.Queues.Models.Enums
{
    public enum MQMessageStatus
    {
        UnProcessed = 0,
        InProcess = 1,
        MarkedForDeletion = 2,
        SubscriberFailed = 3,
        DataError = 4
    }
}
