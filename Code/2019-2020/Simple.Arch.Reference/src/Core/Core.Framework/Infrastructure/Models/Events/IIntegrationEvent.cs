using static Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common.BaseRequestContext;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Models.Events
{
    public interface IIntegrationEvent
    {
        public HeaderContent Header { get; set; }
    }
}
