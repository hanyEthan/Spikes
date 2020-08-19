using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class OrganizationAccountGetRequestMapper : IModelMapper<OrganizationAccountSearchCriteria, OrganizationAccountSearchCriteriaDTO>
    {
        #region props.

        public static OrganizationAccountGetRequestMapper Instance { get; } = new OrganizationAccountGetRequestMapper();

        #endregion
        #region cst.

        static OrganizationAccountGetRequestMapper()
        {
        }
        public OrganizationAccountGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public OrganizationAccountSearchCriteria Map(OrganizationAccountSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            return new OrganizationAccountSearchCriteria()
            {
                AccountIds = from.AccountIds,
                OrganizationIds = from.OrganizationIds,

                #region Common
                Code = from.Code,
                IsActive = from.IsActive,
                Name = from.Name,
                PagingEnabled = from.PagingEnabled,
                PageSize = from.PageSize,
                PageNumber = from.PageNumber,
                SearchIncludes = from.SearchIncludes
                #endregion
            };

        }
        public OrganizationAccountSearchCriteriaDTO Map(OrganizationAccountSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
