using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Mappers.Applications;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Applications.API.Mappers.Applications
{
    public class ApplicationGetResponseMapper : IModelMapper<SearchResults<Application>, SearchResultsDTO<ApplicationDTO>>
    {
        #region props.

        public static ApplicationGetResponseMapper Instance { get; } = new ApplicationGetResponseMapper();

        #endregion      

        #region IModelMapper

        public SearchResults<Application> Map(SearchResultsDTO<ApplicationDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<ApplicationDTO> Map(SearchResults<Application> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<ApplicationDTO>
            {
                Results = Map(from.Results),
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
            };

            return to;
        }

        #endregion
        #region helpers.

        private List<ApplicationDTO> Map(List<Application> from)
        {
            if (from == null) return null;

            var to = new List<ApplicationDTO>();

            foreach (var fromItem in from)
            {
                var toItem = ApplicationMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
