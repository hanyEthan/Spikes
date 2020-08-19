using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Mappers.Skills;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Skills.API.Mappers.Skills
{
    public class SkillGetResponseMapper : IModelMapper<SearchResults<Skill>, SearchResultsDTO<SkillDTO>>
    {
        #region props.

        public static SkillGetResponseMapper Instance { get; } = new SkillGetResponseMapper();

        #endregion      

        #region IModelMapper

        public SearchResults<Skill> Map(SearchResultsDTO<SkillDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<SkillDTO> Map(SearchResults<Skill> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<SkillDTO>
            {
                Results = Map(from.Results),
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
            };

            return to;
        }

        #endregion
        #region helpers.

        private List<SkillDTO> Map(List<Skill> from)
        {
            if (from == null) return null;

            var to = new List<SkillDTO>();

            foreach (var fromItem in from)
            {
                var toItem = SkillMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
       
        #endregion
    }
}
