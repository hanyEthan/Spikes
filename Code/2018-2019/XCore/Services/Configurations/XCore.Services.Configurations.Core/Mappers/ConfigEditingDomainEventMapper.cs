using System;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Events.Domain;

namespace XCore.Services.Configurations.Core.Mappers
{
    public class ConfigEditingDomainEventMapper : IModelMapper<ConfigItem, ConfigEditingDomainEvent>
    {
        #region props.

        public static ConfigEditingDomainEventMapper Instance { get; } = new ConfigEditingDomainEventMapper();

        #endregion
        #region IModelMapper

        public ConfigEditingDomainEvent Map(ConfigItem from, object metadata = null)
        {
            if (from == null) return null;
            var requestContext = metadata as RequestContext;

            var to = new ConfigEditingDomainEvent()
            {
                App = from.AppId.ToString(),
                Module = from.ModuleId.ToString(),
                ConfigCode = from.Code,
                ConfigKey = from.Key,
                ConfigValue = from.Value,
                User = requestContext?.UserId,
            };

            return to;
        }
        public ConfigItem Map(ConfigEditingDomainEvent from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
