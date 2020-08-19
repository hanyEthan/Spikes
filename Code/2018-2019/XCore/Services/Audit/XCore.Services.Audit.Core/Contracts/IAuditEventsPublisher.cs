namespace XCore.Services.Audit.Core.Contracts
{
    public interface IAuditEventsPublisher
    {
        bool? Initialized { get; }
    }
}
