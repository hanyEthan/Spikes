namespace XCore.Framework.Infrastructure.Messaging.Pools.Contracts
{
    public interface IPoolDataUnitySettings
    {
        string DBConnectionName { get; }
        string QueueTableName { get; }
    }
}
