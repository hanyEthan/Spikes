using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Core.Models.Support;
using XCore.Services.Configurations.Models;

namespace XCore.Services.Configurations.Mappers
{
    public class ConfigKeyMapper : IModelMapper<ConfigKeyDTO, ConfigKey>,
                                   IModelMapper<ConfigKey, ConfigKeyDTO>
    {
        #region props.

        public static ConfigKeyMapper Instance { get; } = new ConfigKeyMapper();

        #endregion
        #region IModelMapper

        public ConfigKey Map(ConfigKeyDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigKey()
            {
                App = from.App,
                Key = from.Key,
                Module = from.Module,
            };

            return to;
        }
        public ConfigKeyDTO Map(ConfigKey from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigKeyDTO()
            {
                App = from.App,
                Key = from.Key,
                Module = from.Module,
            };

            return to;
        }

        #endregion
    }
}