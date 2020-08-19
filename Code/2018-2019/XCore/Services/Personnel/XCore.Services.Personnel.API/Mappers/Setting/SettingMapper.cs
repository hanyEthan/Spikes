using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Settings;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.Models.DTO.Essential.Settings;

namespace XCore.Services.Personnel.API.Mappers.Settings
{
    public class SettingMapper : SettingEssentialMapper<Setting, SettingDTO>
    {
        #region props.

        public static SettingMapper Instance { get; } = new SettingMapper();

        #endregion
        #region cst.

        static SettingMapper()
        {
        }
        public SettingMapper()
        {
        }

        #endregion

        #region IModelMapper

        public override SettingDTO Map(Setting from, object metadata = null)
        {
            if (from == null) return null;

            var to = base.Map(from, metadata);

            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            #endregion
            return to;
        }
        public override Setting Map(SettingDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = base.Map(from, metadata);

            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            #endregion
            return to;
        }
        #endregion
    }
}
