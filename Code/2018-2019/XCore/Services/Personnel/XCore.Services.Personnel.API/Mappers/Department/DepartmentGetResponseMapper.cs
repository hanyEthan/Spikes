using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Support;

namespace XCore.Services.Personnel.API.Mappers.Departments
{
    public class DepartmentGetResponseMapper : IModelMapper<SearchResults<Department>, SearchResultsDTO<DepartmentDTO>>
    {
        #region props.

        private DepartmentMapper DepartmentMapper { get; set; } = new DepartmentMapper();

        public static DepartmentGetResponseMapper Instance { get; } = new DepartmentGetResponseMapper();

        #endregion
        #region cst.

        static DepartmentGetResponseMapper()
        {
        }
        public DepartmentGetResponseMapper()
        {
        }

        #endregion

        #region IModelMapper

        public SearchResults<Department> Map(SearchResultsDTO<DepartmentDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<DepartmentDTO> Map(SearchResults<Department> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<DepartmentDTO>();

            to.Results = Map(from.Results);

            return to;
        }

        #endregion
        #region helpers.

        private List<DepartmentDTO> Map(List<Department> from)
        {
            if (from == null) return null;

            var to = new List<DepartmentDTO>();

            foreach (var fromItem in from)
            {
                var toItem = this.DepartmentMapper.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
