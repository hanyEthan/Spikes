using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Settings;
using XCore.Services.Personnel.Models.DTO.Settings;

namespace XCore.Services.Personnel.API.Mappers.Settings
{
    public class SettingGetRequestMapper : IModelMapper<SettingSearchCriteria, SettingSearchCriteriaDTO>
    {
        #region props.

        public static SettingGetRequestMapper Instance { get; } = new SettingGetRequestMapper();

        #endregion
        #region cst.

        static SettingGetRequestMapper()
        {
        }
        public SettingGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SettingSearchCriteria Map(SettingSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SettingSearchCriteria();
            to.SettingIds = from.SettingIds;
            to.AccountIds = from.AccountIds;
            #region Common
            to.Id = from.Id;
            to.Code = from.Code;
            to.IsActive = from.IsActive;
            to.Name = from.Name;
            to.PagingEnabled = from.PagingEnabled;
            to.PageSize = from.PageSize;
            to.PageNumber = from.PageNumber;
            to.SearchIncludes = from.SearchIncludes;
            #endregion

            return to;
        }
        public SettingSearchCriteriaDTO Map(SettingSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
