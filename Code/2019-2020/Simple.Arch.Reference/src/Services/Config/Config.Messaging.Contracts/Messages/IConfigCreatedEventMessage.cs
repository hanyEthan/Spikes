using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;

namespace Config.Messaging.Contracts.Messages
{
    public interface IConfigCreatedEventMessage : IBaseRequestContext
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }
    }
}
