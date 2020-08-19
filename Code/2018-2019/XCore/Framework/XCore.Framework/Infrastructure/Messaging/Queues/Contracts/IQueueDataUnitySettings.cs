namespace XCore.Framework.Infrastructure.Messaging.Queues.Contracts
{
    public interface IQueueDataUnitySettings
    {
        string DBConnectionName { get; }
        string QueueTableName { get; }
    }
}
