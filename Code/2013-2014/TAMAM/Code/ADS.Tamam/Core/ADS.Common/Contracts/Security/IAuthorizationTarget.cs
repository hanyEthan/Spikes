namespace ADS.Common.Contracts.Security
{
    public interface IAuthorizationTarget : IAuthorizationHolder
    {
        string Name { get; set; }
    }
}
