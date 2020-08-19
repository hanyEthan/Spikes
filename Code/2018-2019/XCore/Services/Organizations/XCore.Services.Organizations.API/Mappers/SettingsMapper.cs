using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.Settings;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class SettingsMapper : IModelMapper<SettingsDTO, Settings>,
                             IModelMapper<Settings, SettingsDTO>
    {
        #region props.

        public static SettingsMapper Instance { get; } = new SettingsMapper();

        #endregion
        #region IModelMapper

        public Settings Map(SettingsDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Settings()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                Description = from.Description,
                OrganizationId = from.OrganizationId,
            };

            return to;
        }
        public SettingsDTO Map(Settings from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SettingsDTO()
            {

                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                Description = from.Description,
                OrganizationId = from.OrganizationId,
            };

            return to;
        }

        #endregion

    }
}
