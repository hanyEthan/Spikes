using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Settings;
using XCore.Services.Personnel.Models.DTO.Settings;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.API.Mappers.Settings
{
    public class SettingGetResponseMapper : IModelMapper<SearchResults<Setting>, SearchResultsDTO<SettingDTO>>
    {
        #region props.

        private SettingMapper SettingMapper { get; set; } = new SettingMapper();

        public static SettingGetResponseMapper Instance { get; } = new SettingGetResponseMapper();

        #endregion
        #region cst.

        static SettingGetResponseMapper()
        {
        }
        public SettingGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<Setting> Map(SearchResultsDTO<SettingDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<SettingDTO> Map(SearchResults<Setting> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<SettingDTO>();

            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<SettingDTO> Map(List<Setting> from)
        {
            if (from == null) return null;

            var to = new List<SettingDTO>();

            foreach (var fromItem in from)
            {
                var toItem = this.SettingMapper.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
