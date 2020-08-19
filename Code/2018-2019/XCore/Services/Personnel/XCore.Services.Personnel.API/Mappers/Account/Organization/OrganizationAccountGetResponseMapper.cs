using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class OrganizationAccountGetResponseMapper : IModelMapper<SearchResults<OrganizationAccount>, SearchResultsDTO<OrganizationAccountDTO>>
    {
        #region props.

        private OrganizationAccountMapper AccountMapper { get; set; } = new OrganizationAccountMapper();

        public static OrganizationAccountGetResponseMapper Instance { get; } = new OrganizationAccountGetResponseMapper();

        #endregion
        #region cst.

        static OrganizationAccountGetResponseMapper()
        {
        }
        public OrganizationAccountGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<OrganizationAccount> Map(SearchResultsDTO<OrganizationAccountDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<OrganizationAccountDTO> Map(SearchResults<OrganizationAccount> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<OrganizationAccountDTO>();

            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<OrganizationAccountDTO> Map(List<OrganizationAccount> from)
        {
            if (from == null) return null;

            var to = new List<OrganizationAccountDTO>();

            foreach (var fromItem in from)
            {
                var toItem = this.AccountMapper.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
