using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Mappers.Organizations;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;


namespace XCore.Services.Organizations.API.Mappers.Organizations
{
    public class OrganizationGetResponseMapper : IModelMapper<SearchResults<Organization>, SearchResultsDTO<OrganizationDTO>>
    {
        #region props.

        public static OrganizationGetResponseMapper Instance { get; } = new OrganizationGetResponseMapper();

        #endregion      

        #region IModelMapper

        public SearchResults<Organization> Map(SearchResultsDTO<OrganizationDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<OrganizationDTO> Map(SearchResults<Organization> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<OrganizationDTO>
            {
                Results = Map(from.Results),
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
            };

            return to;
        }

        #endregion
        #region helpers.

        private List<OrganizationDTO> Map(List<Organization> from)
        {
            if (from == null) return null;

            var to = new List<OrganizationDTO>();

            foreach (var fromItem in from)
            {
                var toItem = OrganizationMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
