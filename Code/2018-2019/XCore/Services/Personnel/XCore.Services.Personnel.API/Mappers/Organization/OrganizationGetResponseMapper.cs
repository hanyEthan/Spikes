using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Organizations;
using XCore.Services.Personnel.Models.DTO.Organizations;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.API.Mappers.Organizations
{
    public class OrganizationGetResponseMapper : IModelMapper<SearchResults<Organization>, SearchResultsDTO<OrganizationDTO>>
    {
        #region props.

        private OrganizationMapper OrganizationMapper { get; set; } = new OrganizationMapper();

        public static OrganizationGetResponseMapper Instance { get; } = new OrganizationGetResponseMapper();

        #endregion
        #region cst.

        static OrganizationGetResponseMapper()
        {
        }
        public OrganizationGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<Organization> Map(SearchResultsDTO<OrganizationDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<OrganizationDTO> Map(SearchResults<Organization> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<OrganizationDTO>();

            to.Results = Map(from.Results);

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
                var toItem = this.OrganizationMapper.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
