using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Organizations;
using XCore.Services.Personnel.Models.DTO.Organizations;

namespace XCore.Services.Personnel.API.Mappers.Organizations
{
    public class OrganizationGetRequestMapper : IModelMapper<OrganizationSearchCriteria, OrganizationSearchCriteriaDTO>
    {
        #region props.

        public static OrganizationGetRequestMapper Instance { get; } = new OrganizationGetRequestMapper();

        #endregion
        #region cst.

        static OrganizationGetRequestMapper()
        {
        }
        public OrganizationGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public OrganizationSearchCriteria Map(OrganizationSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationSearchCriteria();
            to.OrganizationReferenceId = from.OrganizationReferenceId;
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
        public OrganizationSearchCriteriaDTO Map(OrganizationSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
