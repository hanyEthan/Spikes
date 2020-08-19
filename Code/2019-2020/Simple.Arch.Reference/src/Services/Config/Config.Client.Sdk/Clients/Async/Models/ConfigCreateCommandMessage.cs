using Config.Messaging.Contracts.Messages;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;

namespace Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Async.Models
{
    public class ConfigCreateCommandMessage : BaseRequestContext, IConfigCreateCommandMessage
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }
    }
}
