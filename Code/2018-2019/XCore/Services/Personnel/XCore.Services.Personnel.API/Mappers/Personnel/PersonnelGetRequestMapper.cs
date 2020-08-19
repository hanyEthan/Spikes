using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Personnels;
using XCore.Services.Personnel.Models.DTO.Personnels;

namespace XCore.Services.Personnel.API.Mappers.Personnels
{
    public class PersonnelGetRequestMapper : IModelMapper<PersonSearchCriteria, PersonnelSearchCriteriaDTO>
    {
        #region props.

        public static PersonnelGetRequestMapper Instance { get; } = new PersonnelGetRequestMapper();

        #endregion
        #region cst.

        static PersonnelGetRequestMapper()
        {
        }
        public PersonnelGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public PersonSearchCriteria Map(PersonnelSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new PersonSearchCriteria();
            to.PersonnelIds = from.PersonnelIds;
            to.ManagerIds = from.ManagerIds;
            to.DepartmentIds = from.DepartmentIds;
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
        public PersonnelSearchCriteriaDTO Map(PersonSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
