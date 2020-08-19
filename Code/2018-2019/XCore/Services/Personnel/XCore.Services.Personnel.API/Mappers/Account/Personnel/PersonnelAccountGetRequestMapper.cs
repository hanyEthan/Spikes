using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class PersonnelAccountGetRequestMapper : IModelMapper<PersonnelAccountSearchCriteria, PersonnelAccountSearchCriteriaDTO>
    {
        #region props.

        public static PersonnelAccountGetRequestMapper Instance { get; } = new PersonnelAccountGetRequestMapper();

        #endregion
        #region cst.

        static PersonnelAccountGetRequestMapper()
        {
        }
        public PersonnelAccountGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public PersonnelAccountSearchCriteria Map(PersonnelAccountSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            return new PersonnelAccountSearchCriteria()
            {
                AccountIds = from.AccountIds,
                PersonIds = from.PersonIds,
                
                #region Common
                Id = from.Id,
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
        public PersonnelAccountSearchCriteriaDTO Map(PersonnelAccountSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
