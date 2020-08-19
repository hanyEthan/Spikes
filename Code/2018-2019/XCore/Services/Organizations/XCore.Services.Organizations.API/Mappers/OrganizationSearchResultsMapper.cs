using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Organization;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class OrganizationSearchResultsMapper : IModelMapper<SearchResults<Organization>, SearchResultsDTO<OrganizationDTO>>
    {
        #region props.

        private OrganizationMapper OrganizationMapper { get; set; } = new OrganizationMapper();
        public static OrganizationSearchResultsMapper Instance { get; } = new OrganizationSearchResultsMapper();

        #endregion
        #region IModelMapper

        public SearchResults<Organization> Map(SearchResultsDTO<OrganizationDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<OrganizationDTO> Map(SearchResults<Organization> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<OrganizationDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<OrganizationDTO> Map(List<Organization> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<OrganizationDTO>();

            foreach (var item in from)
            {
                var toItem = this.OrganizationMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }

}
