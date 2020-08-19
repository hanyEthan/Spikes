using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.DTO.Departments;

namespace XCore.Services.Personnel.API.Mappers.Departments
{
    public class DepartmentGetRequestMapper : IModelMapper<DepartmentSearchCriteria, DepartmentSearchCriteriaDTO>
    {
        #region props.

        public static DepartmentGetRequestMapper Instance { get; } = new DepartmentGetRequestMapper();

        #endregion
        #region cst.

        static DepartmentGetRequestMapper()
        {
        }
        public DepartmentGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public DepartmentSearchCriteria Map(DepartmentSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new DepartmentSearchCriteria();
            to.DepartmentReferenceId = from.DepartmentReferenceId;
            to.HeadDepartmentId = from.HeadDepartmentId;
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
        public DepartmentSearchCriteriaDTO Map(DepartmentSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
