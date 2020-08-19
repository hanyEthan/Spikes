namespace ADS.Common.Contracts
{
    public interface IBaseHandler
    {
        bool Initialized { get; }
        string Name { get; }
    }
}
