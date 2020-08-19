using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Event;

using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
  
    public class EventSearchResultsMapper : IModelMapper<SearchResults<Event>, SearchResultsDTO<EventDTO>>
    {
        #region props.

        private EventMapper EventMapper { get; set; } = new EventMapper();
        public static EventSearchResultsMapper Instance { get; } = new EventSearchResultsMapper();
       

        #endregion
        #region IModelMapper

        public SearchResults<Event> Map(SearchResultsDTO<EventDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<EventDTO> Map(SearchResults<Event> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<EventDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<EventDTO> Map(List<Event> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<EventDTO>();

            foreach (var item in from)
            {
                var toItem =EventMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }

}
