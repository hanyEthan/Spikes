using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Mappers.Candidates;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Candidates.API.Mappers.Candidates
{
    public class CandidateGetResponseMapper : IModelMapper<SearchResults<Candidate>, SearchResultsDTO<CandidateDTO>>
    {
        #region props.

        public static CandidateGetResponseMapper Instance { get; } = new CandidateGetResponseMapper();

        #endregion      

        #region IModelMapper

        public SearchResults<Candidate> Map(SearchResultsDTO<CandidateDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<CandidateDTO> Map(SearchResults<Candidate> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<CandidateDTO>
            {
                Results = Map(from.Results),
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
            };

            return to;
        }

        #endregion
        #region helpers.

        private List<CandidateDTO> Map(List<Candidate> from)
        {
            if (from == null) return null;

            var to = new List<CandidateDTO>();

            foreach (var fromItem in from)
            {
                var toItem = CandidateMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
