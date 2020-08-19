using System;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Events.Domain;

namespace XCore.Services.Configurations.Core.Mappers
{
    public class ConfigEditedDomainEventMapper : IModelMapper<ConfigItem, ConfigEditedDomainEvent>
    {
        #region props.

        public static ConfigEditedDomainEventMapper Instance { get; } = new ConfigEditedDomainEventMapper();

        #endregion
        #region IModelMapper

        public ConfigEditedDomainEvent Map(ConfigItem from, object metadata = null)
        {
            if (from == null) return null;
            var requestContext = metadata as RequestContext;

            var to = new ConfigEditedDomainEvent()
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
        public ConfigItem Map(ConfigEditedDomainEvent from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
