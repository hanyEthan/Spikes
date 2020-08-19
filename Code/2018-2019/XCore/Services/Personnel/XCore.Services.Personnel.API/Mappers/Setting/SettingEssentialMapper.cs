using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Settings;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.Models.DTO.Essential.Settings;

namespace XCore.Services.Personnel.API.Mappers.Settings
{
    public class SettingEssentialMapper<TModel, TModelDTO> : IModelMapper<TModel, TModelDTO>
        where TModel : Setting, new()
        where TModelDTO : SettingEssentialDTO, new()
        
    {
        #region props.

        public static SettingEssentialMapper<TModel, TModelDTO> Instance { get; } = new SettingEssentialMapper<TModel, TModelDTO>();

        #endregion
        #region cst.

        static SettingEssentialMapper()
        {
        }
        public SettingEssentialMapper()
        {
        }

        #endregion

        #region IModelMapper

        public virtual TModelDTO Map(TModel from, object metadata = null)
        {
            if (from == null) return null;

            var to = new TModelDTO();
            to.AccountId = from.AccountId;
            #region Common
            to.Id = from.Id;
            to.Code = from.Code;
            to.IsActive = from.IsActive;
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.MetaData = from.MetaData;
            #endregion
            return to;
        }
        public virtual TModel Map(TModelDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new TModel();
            #region Common
            to.Id = from.Id;
            to.Code = from.Code;
            to.IsActive = from.IsActive ?? true;
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.MetaData = from.MetaData;
            #endregion
            return to;
        }

        #endregion
    }
}
