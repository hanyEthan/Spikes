using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class PersonnelAccountGetResponseMapper : IModelMapper<SearchResults<PersonnelAccount>, SearchResultsDTO<PersonnelAccountDTO>>
    {
        #region props.

        private PersonnelAccountMapper AccountMapper { get; set; } = new PersonnelAccountMapper();

        public static PersonnelAccountGetResponseMapper Instance { get; } = new PersonnelAccountGetResponseMapper();

        #endregion
        #region cst.

        static PersonnelAccountGetResponseMapper()
        {
        }
        public PersonnelAccountGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<PersonnelAccount> Map(SearchResultsDTO<PersonnelAccountDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<PersonnelAccountDTO> Map(SearchResults<PersonnelAccount> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<PersonnelAccountDTO>();

            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<PersonnelAccountDTO> Map(List<PersonnelAccount> from)
        {
            if (from == null) return null;

            var to = new List<PersonnelAccountDTO>();

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
