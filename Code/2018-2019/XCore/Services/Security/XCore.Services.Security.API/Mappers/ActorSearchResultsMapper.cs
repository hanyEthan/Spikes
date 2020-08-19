using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.API.Mappers
{
    public class ActorSearchResultsMapper : IModelMapper<SearchResults<Actor>, SearchResultsDTO<ActorDTO>>
    {
        #region props.

        private ActorMapper ActorMapper { get; set; } = new ActorMapper();
        public static ActorSearchResultsMapper Instance { get; } = new ActorSearchResultsMapper();
        #endregion
        #region IModelMapper

        public SearchResults<Actor> Map(SearchResultsDTO<ActorDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<ActorDTO> Map(SearchResults<Actor> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<ActorDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<ActorDTO> Map(List<Actor> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<ActorDTO>();

            foreach (var item in from)
            {
                var toItem = this.ActorMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
