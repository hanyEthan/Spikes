using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.API.Mappers
{
    public class PrivilegeSearchResultsMapper : IModelMapper<SearchResults<Privilege>, SearchResultsDTO<PrivilegeDTO>>
    {
        #region props.

        private PrivilegeMapper PrivilegeMapper { get; set; } = new PrivilegeMapper();
        public static PrivilegeSearchResultsMapper Instance { get; } = new PrivilegeSearchResultsMapper();
        #endregion
        #region IModelMapper

        public SearchResults<Privilege> Map(SearchResultsDTO<PrivilegeDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<PrivilegeDTO> Map(SearchResults<Privilege> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<PrivilegeDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<PrivilegeDTO> Map(List<Privilege> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<PrivilegeDTO>();

            foreach (var item in from)
            {
                var toItem = this.PrivilegeMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
