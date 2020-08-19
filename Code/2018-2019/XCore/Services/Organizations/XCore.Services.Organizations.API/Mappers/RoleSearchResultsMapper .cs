using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Role;

using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class RoleSearchResultsMapper : IModelMapper<SearchResults<Role>, SearchResultsDTO<RoleDTO>>
    {
        #region props.

        private RoleMapper RoleMapper { get; set; } = new RoleMapper();
        public static RoleSearchResultsMapper Instance { get; } = new RoleSearchResultsMapper();
       

        #endregion
        #region IModelMapper

        public SearchResults<Role> Map(SearchResultsDTO<RoleDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<RoleDTO> Map(SearchResults<Role> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<RoleDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<RoleDTO> Map(List<Role> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<RoleDTO>();

            foreach (var item in from)
            {
                var toItem =RoleMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }

}
