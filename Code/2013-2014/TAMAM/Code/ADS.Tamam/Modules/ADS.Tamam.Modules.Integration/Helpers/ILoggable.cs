namespace ADS.Tamam.Modules.Integration.Helpers
{
    public interface ILoggable
    {
        string Reference { get; }
        string GetLoggingData();
    }
}