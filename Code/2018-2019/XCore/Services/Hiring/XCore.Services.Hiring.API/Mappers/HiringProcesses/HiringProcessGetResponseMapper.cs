using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Mappers.HiringProcesses;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;


namespace XCore.Services.HiringProcesses.API.Mappers.HiringProcesses
{
    public class HiringProcessGetResponseMapper : IModelMapper<SearchResults<HiringProcess>, SearchResultsDTO<HiringProcessDTO>>
    {
        #region props.

        public static HiringProcessGetResponseMapper Instance { get; } = new HiringProcessGetResponseMapper();

        #endregion      

        #region IModelMapper

        public SearchResults<HiringProcess> Map(SearchResultsDTO<HiringProcessDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<HiringProcessDTO> Map(SearchResults<HiringProcess> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<HiringProcessDTO>
            {
                Results = Map(from.Results),
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
            };

            return to;
        }

        #endregion
        #region helpers.

        private List<HiringProcessDTO> Map(List<HiringProcess> from)
        {
            if (from == null) return null;

            var to = new List<HiringProcessDTO>();

            foreach (var fromItem in from)
            {
                var toItem = HiringProcessMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
