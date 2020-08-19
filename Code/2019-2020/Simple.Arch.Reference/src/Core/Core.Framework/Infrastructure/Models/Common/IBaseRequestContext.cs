namespace Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common
{
    public interface IBaseRequestContext
    {
        BaseRequestContext.HeaderContent Header { get; set; }
    }
}
