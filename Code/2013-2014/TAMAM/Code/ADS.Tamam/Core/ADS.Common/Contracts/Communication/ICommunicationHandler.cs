namespace ADS.Common.Contracts.Communication
{
    public interface ICommunicationHandler : IBaseHandler
    {
        string EncryptQueryString(string url);
        string DecryptQueryString(string url);
    }
}