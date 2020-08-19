using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.API.Mappers
{
    public class ConfigMapper : IModelMapper<ConfigDTO, ConfigItem>,
                             IModelMapper<ConfigItem, ConfigDTO>
    {
        #region IModelMapper

        public ConfigItem Map(ConfigDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigItem()
            {
                Key=from.Key,
                Value = from.Value,
                Type=from.Type,
                ReadOnly=from.ReadOnly,
                Version =from.Version,
                ModuleId = from.ModuleId,
                AppId =from.AppId,
                Module = ModuleMapper.Instance.Map(from.Module, metadata = null),
                App = AppMapper.Instance.Map(from.App, metadata = null),

                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateFormat),
                Description = from.Description,
                IsActive = from.IsActive ?? true,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,

            };

            return to;
        }
        public ConfigDTO Map(ConfigItem from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigDTO()
            {
                Key = from.Key,
                Value = from.Value,
                Type = from.Type,
                ReadOnly = from.ReadOnly,
                Version = from.Version,
                ModuleId = from.ModuleId,
                AppId = from.AppId,
                Module = ModuleMapper.Instance.Map(from.Module, metadata = null),
                App = AppMapper.Instance.Map(from.App, metadata = null),



                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat),
                Description = from.Description,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
            };

            return to;
        }

        #endregion
    }
}