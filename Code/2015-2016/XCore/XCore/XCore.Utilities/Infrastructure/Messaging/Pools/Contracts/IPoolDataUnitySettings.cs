namespace XCore.Utilities.Infrastructure.Messaging.Pools.Contracts
{
    public interface IPoolDataUnitySettings
    {
        string DBConnectionName { get; }
        string QueueTableName { get; }
    }
}
