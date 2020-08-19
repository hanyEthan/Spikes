namespace XCore.Framework.Framework.Unity.Contracts
{
    public interface IUnityService
    {
        bool? Initialized { get; }
        string ServiceId { get; }
    }
}
