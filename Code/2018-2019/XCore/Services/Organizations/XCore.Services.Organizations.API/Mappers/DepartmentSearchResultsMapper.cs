using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class DepartmentSearchResultsMapper : IModelMapper<SearchResults<Department>, SearchResultsDTO<DepartmentDTO>>
    {
        #region props.

        private DepartmentMapper DepartmentMapper { get; set; } = new DepartmentMapper();
        public static DepartmentSearchResultsMapper Instance { get; } = new DepartmentSearchResultsMapper();

        #endregion
        #region IModelMapper

        public SearchResults<Department> Map(SearchResultsDTO<DepartmentDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<DepartmentDTO> Map(SearchResults<Department> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<DepartmentDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<DepartmentDTO> Map(List<Department> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<DepartmentDTO>();

            foreach (var item in from)
            {
                var toItem = this.DepartmentMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
