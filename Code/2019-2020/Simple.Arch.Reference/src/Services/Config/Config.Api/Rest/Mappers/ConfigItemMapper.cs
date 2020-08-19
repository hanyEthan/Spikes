using Mcs.Invoicing.Services.Config.Api.Rest.Models;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Api.Rest.Mappers
{
    public class ConfigItemMapper : IModelMapper<ConfigItem, ConfigItemDTO>
    {
        #region IModelMapper

        public ConfigItemDTO Map(ConfigItem from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigItemDTO()
            {
                Id = from.Id,
                Key = from.Key,
                Value = from.Value,
                Description = from.Description,
                ModuleId = from.ModuleId,
                ModuleName = from.Module?.Name,

                IsDeleted = from.IsDeleted,
                CreatedByUserId = from.CreatedByUserId,
                CreationDateTimeUtc = from.CreationDateTimeUtc,
                LastModifiedByUserId = from.LastModifiedByUserId,
                LastModificationDateTimeUtc = from.LastModificationDateTimeUtc,
            };

            return to;
        }
        public TDestinationAlt Map<TDestinationAlt>(ConfigItem from, object metadata = null) where TDestinationAlt : ConfigItemDTO
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
