using Config.Messaging.Contracts.Messages;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;

namespace Mcs.Invoicing.Services.Config.Domain.Events
{
    public class ConfigCreatedEvent : BaseRequestContext, IConfigCreatedEventMessage
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }
    }
}
