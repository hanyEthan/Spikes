using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.OrganizationDelegation;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class OrganizationDelegationSearchResultsMapper : IModelMapper<SearchResults<OrganizationDelegation>, SearchResultsDTO<OrganizationDelegationDTO>>
    {
        #region props.

        private OrganizationDelegationMapper OrganizationDelegationMapper { get; set; } = new OrganizationDelegationMapper();
        public static OrganizationDelegationSearchResultsMapper Instance { get; } = new OrganizationDelegationSearchResultsMapper();

        #endregion
        #region IModelMapper
        public SearchResults<OrganizationDelegation> Map(SearchResultsDTO<OrganizationDelegationDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        public SearchResultsDTO<OrganizationDelegationDTO> Map(SearchResults<OrganizationDelegation> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<OrganizationDelegationDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<OrganizationDelegationDTO> Map(List<OrganizationDelegation> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<OrganizationDelegationDTO>();

            foreach (var item in from)
            {
                var toItem = this.OrganizationDelegationMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

       


        #endregion
    }
}
