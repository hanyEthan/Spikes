using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem
{
    public class CreateConfigItemCommandMapper : IModelMapper<CreateConfigItemCommand, ConfigItem>
    {
        #region IModelMapper

        public ConfigItem Map(CreateConfigItemCommand from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigItem()
            {
                Key = from.Key,
                Value = from.Value,
                ModuleId = from.ModuleId,
                Description = from.Description,
            };

            return to;
        }
        public TDestinationAlt Map<TDestinationAlt>(CreateConfigItemCommand from, object metadata = null) where TDestinationAlt : ConfigItem
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
