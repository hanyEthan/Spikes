namespace Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common
{
    public enum ResponseCode
    {
        Success = 100,
        SystemError = 110,
        ValidationError = 120,
        NotFound = 130,
        InvalidInput = 140,

        AuthenticationError = 160,
        AccessLocked = 170,
        AccessDenied = 180,
    }
}
